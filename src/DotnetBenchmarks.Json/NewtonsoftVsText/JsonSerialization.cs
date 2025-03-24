using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;
using Newtonsoft.Json.Serialization;

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
    [Params(10000)]
    public int Count { get; set; }

    private List<User> _testUsers =  [ ];

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

        _testUsers = faker.Generate(Count);
    }

    [BenchmarkCategory("Serialize Big Data"), Benchmark(Baseline = true)]
    public void NewtonsoftSerializeBigData() =>
        _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers);

    [BenchmarkCategory("Serialize Big Data"), Benchmark]
    public void MicrosoftSerializeBigData() =>
        _ = System.Text.Json.JsonSerializer.Serialize(_testUsers);

    [BenchmarkCategory("Serialize Big Data With Settings"), Benchmark(Baseline = true)]
    public void NewtonsoftSerializeBigDataWithSettings()
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers, settings);
    }

    [BenchmarkCategory("Serialize Big Data With Settings"), Benchmark]
    public void MicrosoftSerializeBigDataWithSettings()
    {
        var settings = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
        };

        _ = System.Text.Json.JsonSerializer.Serialize(_testUsers, settings);
    }

    [BenchmarkCategory("Serialize Individual Data"), Benchmark(Baseline = true)]
    public void NewtonsoftSerializeIndividualData()
    {
        foreach (var user in _testUsers)
        {
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        }
    }

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void MicrosoftSerializeIndividualData()
    {
        foreach (var user in _testUsers)
        {
            _ = System.Text.Json.JsonSerializer.Serialize(user);
        }
    }
}
