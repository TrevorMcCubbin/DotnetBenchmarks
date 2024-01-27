using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace NetPerformance.Benchmarks;

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
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD, Column.AllocRatio)]
internal class BenchmarkTemplate
{
    [Params(1000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup() { }

    [Benchmark]
    public void Benchmark() { }
}
