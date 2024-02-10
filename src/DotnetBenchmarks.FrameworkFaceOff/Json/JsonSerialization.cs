using System;
using System.Collections.Generic;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Bogus;
using DotnetBenchmarks.FrameworkFaceOff.Model;

namespace DotnetBenchmarks.FrameworkFaceOff.Json
{
    [SimpleJob(
        RuntimeMoniker.Net48,
        launchCount: 1,
        warmupCount: 3,
        iterationCount: 5,
        invocationCount: -1,
        id: "NET Framework 4.8",
        baseline: true
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
    [Orderer(SummaryOrderPolicy.SlowestToFastest, MethodOrderPolicy.Declared)]
    public class JsonSerialization
    {
        [Params(10000)]
        public int Count { get; set; }

        private List<User> _testUsers = new List<User>();

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

            _testUsers = faker.Generate(Count);
        }

        [
            BenchmarkCategory("Serialize Big Data"),
            Benchmark(Description = "Newtonsoft", Baseline = true)
        ]
        public void NewtonsoftSerializeBigData() =>
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers);

        [BenchmarkCategory("Serialize Big Data"), Benchmark(Description = "Microsoft")]
        public void MicrosoftSerializeBigData() => _ = JsonSerializer.Serialize(_testUsers);

        [
            BenchmarkCategory("Serialize Individual Data"),
            Benchmark(Description = "Newtonsoft", Baseline = true)
        ]
        public void NewtonsoftSerializeIndividualData()
        {
            foreach (var user in _testUsers)
            {
                _ = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            }
        }

        [BenchmarkCategory("Serialize Individual Data"), Benchmark(Description = "Microsoft")]
        public void MicrosoftSerializeIndividualData()
        {
            foreach (var user in _testUsers)
            {
                _ = JsonSerializer.Serialize(user);
            }
        }
    }
}
