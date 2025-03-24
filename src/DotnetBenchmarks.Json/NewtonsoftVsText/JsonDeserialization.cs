using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.Json.Model;
using Newtonsoft.Json;
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
public class JsonDeserialization
{
    private readonly List<string> serializedTestUsersList =  [ ];

    private string serializedTestUsers = string.Empty;

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

        var testUsers = faker.Generate(Count);

        serializedTestUsers = JsonSerializer.Serialize(testUsers);

        foreach (var user in testUsers.Select(u => JsonSerializer.Serialize(u)))
        {
            serializedTestUsersList.Add(user);
        }
    }

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark]
    public void NewtonsoftDeserializeLargeDataset() =>
        _ = JsonConvert.DeserializeObject<List<User>>(serializedTestUsers);

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark(Baseline = true)]
    public void MicrosoftDeserializeLargeDataset() =>
        _ = JsonSerializer.Deserialize<List<User>>(serializedTestUsers);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void NewtonsoftDeserializeIndividualData()
    {
        foreach (var user in serializedTestUsersList)
        {
            _ = JsonConvert.DeserializeObject<User>(user);
        }
    }

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark(Baseline = true)]
    public void MicrosoftDeserializeIndividualData()
    {
        foreach (var user in serializedTestUsersList)
        {
            _ = JsonSerializer.Deserialize<User>(user);
        }
    }
}
