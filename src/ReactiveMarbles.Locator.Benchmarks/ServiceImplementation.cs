using System.Threading.Tasks;

namespace ReactiveMarbles.Locator.Benchmarks
{
    public class ServiceImplementation : IService, IServices
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
    public class Implementation : IServices
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}