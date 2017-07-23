using System;
using System.Threading.Tasks;

namespace SimpleScheduler.Jobs
{
    public class Test1 : IJob
    {
        public async Task Execute()
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss,fff} Job \"{GetType().Name}\" is executed.");
        }
    }
}
