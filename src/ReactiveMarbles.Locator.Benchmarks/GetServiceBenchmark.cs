using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace ReactiveMarbles.Locator.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GetServiceBenchmark
    {
        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Splat")]
        public void SplatTypeOfGet()
        {
            Splat.Locator.CurrentMutable.Register(() => new ServiceImplementation(), typeof(IService));

            var _ = Splat.Locator.Current.GetService(typeof(IService));
        }

        [Benchmark]
        [BenchmarkCategory("ServiceLocator")]
        public void LocatorGenericGet()
        {
            // ServiceLocator.Current().AddService<IService>(() => new ServiceImplementation());
            //
            // var _ = ServiceLocator.Current().GetService<IService>();
        }
    }
}