using System.Threading.Tasks;

namespace SimpleScheduler
{
    public interface IJob
    {
        Task Execute();
    }
}
