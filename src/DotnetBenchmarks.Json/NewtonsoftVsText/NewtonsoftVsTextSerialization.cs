using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;
using Newtonsoft.Json.Serialization;

namespace DotnetBenchmarks.Json.NewtonsoftVsText;

[RPlotExporter]
[SimpleJob(
    RuntimeMoniker.Net80,
    launchCount: 1,
    warmupCount: 3,
    iterationCount: 5,
    invocationCount: -1,
    id: "NET 8.0",
    baseline: true
)]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD)]
public class NewtonsoftVsTextSerialization
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

    [Benchmark]
    public void NewtonsoftSerializeBigData() =>
        _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers);

    [Benchmark]
    public void MicrosoftSerializeBigData() =>
        _ = System.Text.Json.JsonSerializer.Serialize(_testUsers);

    [Benchmark]
    public void NewtonsoftSerializeBigDataWithSettings()
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers, settings);
    }

    [Benchmark]
    public void MicrosoftSerializeBigDataWithSettings()
    {
        var settings = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
        };

        _ = System.Text.Json.JsonSerializer.Serialize(_testUsers, settings);
    }

    [Benchmark]
    public void NewtonsoftSerializeIndividualData()
    {
        foreach (var user in _testUsers)
        {
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        }
    }

    [Benchmark]
    public void MicrosoftSerializeIndividualData()
    {
        foreach (var user in _testUsers)
        {
            _ = System.Text.Json.JsonSerializer.Serialize(user);
        }
    }
}
