using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Locator.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60)]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("Reactive Marbles")]
public class ServiceLocatorGetServicesBenchmark
{
    [GlobalSetup]
    public static void Setup()
    {
        ServiceLocator.Current().AddService<IServices>(() => new Implementation());
        ServiceLocator.Current().AddService<IServices>(() => new ServiceImplementation());
        ServiceLocator.Current().AddService<IServices>(() => new Implementation(), Contract);
        ServiceLocator.Current().AddService<IServices>(() => new ServiceImplementation(), Contract);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Performance")]
    public void GetServices()
    {
        var _ = ServiceLocator.Current().GetServices<IServices>();
    }

    [Benchmark]
    [BenchmarkCategory("Performance")]
    public void GetServicesWithContract()
    {
        var _ = ServiceLocator.Current().GetServices<IServices>(Contract);
    }

    private const string Contract = "contract";
}