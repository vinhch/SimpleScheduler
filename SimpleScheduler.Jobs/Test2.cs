using System;
using System.Threading.Tasks;

namespace SimpleScheduler.Jobs
{
    public class Test2 : IJob
    {
        public async Task Execute()
        {
            Console.WriteLine($"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss,fffzzz} Job \"{GetType().Name}\" is executed.");
        }
    }
}
