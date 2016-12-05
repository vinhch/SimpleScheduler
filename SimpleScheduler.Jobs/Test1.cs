using System;

namespace SimpleScheduler.Jobs
{
    public class Test1 : IJob
    {
        public void Execute()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff")} Job \"{GetType().Name}\" is executed.");
        }
    }
}
