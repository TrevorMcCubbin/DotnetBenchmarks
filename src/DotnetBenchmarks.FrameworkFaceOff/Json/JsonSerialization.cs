using System;
using System.Collections.Generic;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using DotnetBenchmarks.FrameworkFaceOff.Model;

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

        [BenchmarkCategory("Serialize Big Data"), Benchmark(Baseline = true)]
        public void NewtonsoftSerializeBigData() =>
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers);

        [BenchmarkCategory("Serialize Big Data"), Benchmark]
        public void MicrosoftSerializeBigData() => _ = JsonSerializer.Serialize(_testUsers);

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
                _ = JsonSerializer.Serialize(user);
            }
        }
    }
}
