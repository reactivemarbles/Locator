using System.Threading.Tasks;

namespace ReactiveMarbles.Locator.Benchmarks
{
    public interface IService
    {
        Task Get();
    }

    public interface IServices
    {
        Task Get();
    }
}
