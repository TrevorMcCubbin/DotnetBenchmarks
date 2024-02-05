using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.FrameworkFaceOff.Model;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DotnetBenchmarks.FrameworkFaceOff.Json
{
    [RPlotExporter]
    [SimpleJob(
        RuntimeMoniker.Net48,
        launchCount: 1,
        warmupCount: 3,
        iterationCount: 5,
        invocationCount: -1,
        id: "NET Framework 4.8"
    )]
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

        private readonly List<string> _serializedTestUsersList = new List<string>();

        [GlobalSetup]
        public void GlobalSetup()
        {
            var faker = new Faker<User>().CustomInstantiator(
                f =>
                    new User
                    {
                        UserId = Guid.NewGuid(),
                        FirstName = f.Name.FirstName(),
                        LastName = f.Name.LastName(),
                        FullName = f.Name.FullName(),
                        Username = f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                        Email = f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
                    }
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
            _ = JsonConvert.DeserializeObject<List<User>>(_serializedTestUsers);

        [BenchmarkCategory("Deserialize Big Data"), Benchmark]
        public void MicrosoftDeserializeBigData() =>
            _ = JsonSerializer.Deserialize<List<User>>(_serializedTestUsers);

        [BenchmarkCategory("Deserialize Individual Data"), Benchmark(Baseline = true)]
        public void NewtonsoftDeserializeIndividualData()
        {
            foreach (var user in _serializedTestUsersList)
            {
                _ = JsonConvert.DeserializeObject<User>(user);
            }
        }

        [BenchmarkCategory("Deserialize Individual Data"), Benchmark]
        public void MicrosoftDeserializeIndividualData()
        {
            foreach (var user in _serializedTestUsersList)
            {
                _ = JsonSerializer.Deserialize<User>(user);
            }
        }
    }
}
