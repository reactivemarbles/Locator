using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Locator.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [BenchmarkCategory("Splat")]
    public class LocatorGetServiceBenchmark
    {
        [GlobalSetup]
        public static void Setup()
        {
            Splat.Locator.CurrentMutable.Register(() => new ServiceImplementation(), typeof(IService));
            Splat.Locator.CurrentMutable.Register(() => new ServiceImplementation(), typeof(IService), Contract);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Performance")]
        public void GetService()
        {
            var _ = Splat.Locator.Current.GetService(typeof(IService));
        }

        [Benchmark]
        [BenchmarkCategory("Performance")]
        public void GetServiceWithContract()
        {
            var _ = Splat.Locator.Current.GetService(typeof(IService), Contract);
        }

        private const string Contract = "contract";
    }
}