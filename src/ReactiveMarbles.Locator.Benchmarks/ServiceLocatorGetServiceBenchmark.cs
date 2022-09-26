using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Locator.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("Reactive Marbles")]
public class ServiceLocatorGetServiceBenchmark
{
    [GlobalSetup]
    public static void Setup()
    {
        ServiceLocator.Current().AddService<IService>(() => new ServiceImplementation());
        ServiceLocator.Current().AddService<IService>(() => new ServiceImplementation(), Contract);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Performance")]
    public void GetService()
    {
        var _ = ServiceLocator.Current().GetService<IService>();
    }

    [Benchmark]
    [BenchmarkCategory("Performance")]
    public void GetServiceWithContract()
    {
        var _ = ServiceLocator.Current().GetService<IService>(Contract);
    }

    private const string Contract = "contract";
}