using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;

namespace DotnetBenchmarks.Json.NewtonsoftVsText;

[RPlotExporter]
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
public class JsonDeserialization
{
    [Params(10000)]
    public int Count { get; set; }

    private string _serializedTestUsers = string.Empty;

    private readonly List<string> _serializedTestUsersList =  [ ];

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

        var testUsers = faker.Generate(Count);

        _serializedTestUsers = JsonSerializer.Serialize(testUsers);

        foreach (var user in testUsers.Select(u => JsonSerializer.Serialize(u)))
        {
            _serializedTestUsersList.Add(user);
        }
    }

    [BenchmarkCategory("Deserialize Big Data"), Benchmark(Baseline = true)]
    public void NewtonsoftDeserializeBigData() =>
        _ = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(_serializedTestUsers);

    [BenchmarkCategory("Deserialize Big Data"), Benchmark]
    public void MicrosoftDeserializeBigData() =>
        _ = System.Text.Json.JsonSerializer.Deserialize<List<User>>(_serializedTestUsers);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark(Baseline = true)]
    public void NewtonsoftDeserializeIndividualData()
    {
        foreach (var user in _serializedTestUsersList)
        {
            _ = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(user);
        }
    }

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void MicrosoftDeserializeIndividualData()
    {
        foreach (var user in _serializedTestUsersList)
        {
            _ = System.Text.Json.JsonSerializer.Deserialize<User>(user);
        }
    }
}
