using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DotnetBenchmarks.Json.NewtonsoftVsText;

[RPlotExporter]
[SimpleJob(
    RuntimeMoniker.Net90,
    launchCount: 1,
    warmupCount: 3,
    iterationCount: 5,
    invocationCount: -1,
    id: "NET 9.0"
)]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class JsonSerialization
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { WriteIndented = true, PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy() };

    private readonly JsonSerializerSettings jsonSerializerSettings =
        new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

    private List<User> testUsers =  [ ];

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

        testUsers = faker.Generate(Count);
    }

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark]
    public void NewtonsoftSerializeLargeDataset() => _ = JsonConvert.SerializeObject(testUsers);

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark(Baseline = true)]
    public void MicrosoftSerializeLargeDataset() => _ = JsonSerializer.Serialize(testUsers);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark]
    public void NewtonsoftSerializeLargeDatasetWithSettings()
    {
        _ = JsonConvert.SerializeObject(testUsers, jsonSerializerSettings);
    }

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark(Baseline = true)]
    public void MicrosoftSerializeLargeDatasetWithSettings()
    {
        _ = JsonSerializer.Serialize(testUsers, jsonSerializerOptions);
    }

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void NewtonsoftSerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonConvert.SerializeObject(user);
        }
    }

    [BenchmarkCategory("Serialize Individual Data"), Benchmark(Baseline = true)]
    public void MicrosoftSerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonSerializer.Serialize(user);
        }
    }
}
