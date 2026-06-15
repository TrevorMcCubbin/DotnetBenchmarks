using System.Text;
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
public class JsonDeserialization
{
    private readonly List<string> serializedTestUsersList =  [ ];
    private readonly List<byte[]> serializedTestUsersUtf8List =  [ ];

    private string serializedTestUsers = string.Empty;
    private byte[] serializedTestUsersUtf8 = null!;

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
        serializedTestUsersUtf8 = Encoding.UTF8.GetBytes(serializedTestUsers);

        foreach (var user in testUsers.Select(u => JsonSerializer.Serialize(u)))
        {
            serializedTestUsersList.Add(user);
        }
        foreach (var userUtf8 in testUsers.Select(u => JsonSerializer.SerializeToUtf8Bytes(u)))
        {
            serializedTestUsersUtf8List.Add(userUtf8);
        }
    }

    #region Newtonsoft.Json

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark]
    public void Newtonsoft_DeserializeLargeDataset() =>
        _ = JsonConvert.DeserializeObject<List<User>>(serializedTestUsers);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void Newtonsoft_DeserializeIndividualData()
    {
        foreach (var user in serializedTestUsersList)
        {
            _ = JsonConvert.DeserializeObject<User>(user);
        }
    }

    #endregion
    #region System.Text.Json (with Reflection)

    // strings

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark(Baseline = true)]
    public void Microsoft_DeserializeLargeDataset() =>
        _ = JsonSerializer.Deserialize<List<User>>(serializedTestUsers);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark(Baseline = true)]
    public void Microsoft_DeserializeIndividualData()
    {
        foreach (var user in serializedTestUsersList)
        {
            _ = JsonSerializer.Deserialize<User>(user);
        }
    }

    // UTF-8 bytes

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark]
    public void Microsoft_Utf8_DeserializeLargeDataset() =>
        _ = JsonSerializer.Deserialize<List<User>>(serializedTestUsersUtf8);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void Microsoft_Utf8_DeserializeIndividualData()
    {
        foreach (var userUtf8 in serializedTestUsersUtf8List)
        {
            _ = JsonSerializer.Deserialize<User>(userUtf8);
        }
    }

    #endregion
    #region System.Text.Json (with Source Generation)

    // strings

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark]
    public void Microsoft_SourceGen_DeserializeLargeDataset() =>
        _ = JsonSerializer.Deserialize(serializedTestUsers, UserSerializerContext.Default.ListUser);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void Microsoft_SourceGen_DeserializeIndividualData()
    {
        foreach (var user in serializedTestUsersList)
        {
            _ = JsonSerializer.Deserialize(user, UserSerializerContext.Default.User);
        }
    }

    // UTF-8 bytes

    [BenchmarkCategory("Deserialize Large Dataset"), Benchmark]
    public void Microsoft_SourceGen_Utf8_DeserializeLargeDataset() =>
        _ = JsonSerializer.Deserialize(serializedTestUsersUtf8, UserSerializerContext.Default.ListUser);

    [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
    public void Microsoft_SourceGen_Utf8_DeserializeIndividualData()
    {
        foreach (var userUtf8 in serializedTestUsersUtf8List)
        {
            _ = JsonSerializer.Deserialize(userUtf8, UserSerializerContext.Default.User);
        }
    }
    #endregion

}
