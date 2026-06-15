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
    RuntimeMoniker.Net10_0,
    launchCount: 1,
    warmupCount: 3,
    iterationCount: 5,
    invocationCount: -1,
    id: "NET 10.0"
)]
[MemoryDiagnoser(displayGenColumns: false)]
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class JsonSerialization
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

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

    #region Newtonsoft.Json

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark]
    public void Newtonsoft_SerializeLargeDataset() =>
        _ = JsonConvert.SerializeObject(testUsers);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark]
    public void Newtonsoft_SerializeLargeDatasetWithSettings() =>
        _ = JsonConvert.SerializeObject(testUsers, jsonSerializerSettings);

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void Newtonsoft_SerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonConvert.SerializeObject(user);
        }
    }

    #endregion
    #region System.Text.Json (with Reflection)

    // strings

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark(Baseline = true)]
    public void Microsoft_SerializeLargeDataset() =>
        _ = JsonSerializer.Serialize(testUsers);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark(Baseline = true)]
    public void Microsoft_SerializeLargeDatasetWithSettings() =>
        _ = JsonSerializer.Serialize(testUsers, jsonSerializerOptions);

    [BenchmarkCategory("Serialize Individual Data"), Benchmark(Baseline = true)]
    public void Microsoft_SerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonSerializer.Serialize(user);
        }
    }

    // UTF-8 bytes

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark]
    public void Microsoft_Utf8_SerializeLargeDataset() =>
        _ = JsonSerializer.SerializeToUtf8Bytes(testUsers);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark]
    public void Microsoft_Utf8_SerializeLargeDatasetWithSettings() =>
        _ = JsonSerializer.SerializeToUtf8Bytes(testUsers, jsonSerializerOptions);

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void Microsoft_Utf8_SerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(user);
        }
    }

    #endregion
    #region System.Text.Json (with Source Generation)

    // strings

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark]
    public void Microsoft_SourceGen_SerializeLargeDataset() =>
        _ = JsonSerializer.Serialize(testUsers, UserSerializerContext.Default.ListUser);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark]
    public void Microsoft_SourceGen_SerializeLargeDatasetWithSettings() =>
        _ = JsonSerializer.Serialize(testUsers, UserSerializerContextWithOptions.Default.ListUser);

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void Microsoft_SourceGen_SerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonSerializer.Serialize(user, UserSerializerContext.Default.User);
        }
    }

    // UTF-8 bytes

    [BenchmarkCategory("Serialize Large Dataset"), Benchmark]
    public void Microsoft_SourceGen_Utf8_SerializeLargeDataset() =>
        _ = JsonSerializer.SerializeToUtf8Bytes(testUsers, UserSerializerContext.Default.ListUser);

    [BenchmarkCategory("Serialize Large Dataset With Settings"), Benchmark]
    public void Microsoft_SourceGen_Utf8_SerializeLargeDatasetWithSettings() =>
        _ = JsonSerializer.SerializeToUtf8Bytes(testUsers, UserSerializerContextWithOptions.Default.ListUser);

    [BenchmarkCategory("Serialize Individual Data"), Benchmark]
    public void Microsoft_SourceGen_Utf8_SerializeIndividualData()
    {
        foreach (var user in testUsers)
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(user, UserSerializerContext.Default.User);
        }
    }

    #endregion
}
