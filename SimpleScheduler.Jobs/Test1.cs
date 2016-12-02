using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
