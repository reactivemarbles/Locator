using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Locator.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class LocatorGetServicesBenchmark
    {
        [GlobalSetup]
        public static void Setup()
        {
            Splat.Locator.CurrentMutable.Register(() => new Implementation(), typeof(IServices));
            Splat.Locator.CurrentMutable.Register(() => new ServiceImplementation(), typeof(IServices));
            Splat.Locator.CurrentMutable.Register(() => new ServiceImplementation(), typeof(IServices), Contract);
            Splat.Locator.CurrentMutable.Register(() => new Implementation(), typeof(IServices), Contract);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Performance")]
        public void GetServices()
        {
            var _ = Splat.Locator.Current.GetServices(typeof(IServices));
        }

        [Benchmark]
        [BenchmarkCategory("Performance")]
        public void GetServicesWithContract()
        {
            var _ = Splat.Locator.Current.GetServices(typeof(IServices), Contract);
        }

        private const string Contract = "contract";
    }
}