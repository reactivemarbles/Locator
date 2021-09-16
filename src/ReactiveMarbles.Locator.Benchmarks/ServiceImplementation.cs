using System.Threading.Tasks;

namespace ReactiveMarbles.Locator.Benchmarks
{
    public class ServiceImplementation : IService
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}