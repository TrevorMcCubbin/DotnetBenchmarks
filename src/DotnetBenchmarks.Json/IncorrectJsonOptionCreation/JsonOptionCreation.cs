using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotnetBenchmarks.Json.IncorrectJsonOptionCreation;

[SimpleJob(
    RuntimeMoniker.Net80,
    launchCount: 1,
    warmupCount: 3,
    iterationCount: 5,
    invocationCount: -1,
    id: "NET 8.0"
)]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class JsonOptionCreation
{
    private readonly JsonSerializerSettings _newtonsoftSerializerSettings =
        new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

    private readonly JsonSerializerOptions _microsoftSerializerOptions =
        new() { WriteIndented = true, PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy() };

    private List<User> _users =  [ ];

    [Params(10000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var faker = new Faker<User>().CustomInstantiator(
            f =>
                new User(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Name.FullName(),
                    f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                    f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
                )
        );

        _users = faker.Generate(Count);
    }

    [BenchmarkCategory("Newtonsoft With Settings"), Benchmark(Baseline = true)]
    public void NewtonsoftSerializeWithSettingsInitialization()
    {
        JsonConvert.SerializeObject(_users, _newtonsoftSerializerSettings);
    }

    [BenchmarkCategory("Newtonsoft With Settings"), Benchmark]
    public void NewtonsoftSerializeWithInvalidSettingsInitialization()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        JsonConvert.SerializeObject(_users, settings);
    }

    [BenchmarkCategory("Microsoft With Settings"), Benchmark(Baseline = true)]
    public void MicrosoftSerializeWithSettingsInitialization()
    {
        System.Text.Json.JsonSerializer.Serialize(_users, _microsoftSerializerOptions);
    }

    [BenchmarkCategory("Microsoft With Settings"), Benchmark]
    public void MicrosoftSerializeWithInvalidSettingsInitialization()
    {
        var settings = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
        };

        System.Text.Json.JsonSerializer.Serialize(_users, settings);
    }
}
