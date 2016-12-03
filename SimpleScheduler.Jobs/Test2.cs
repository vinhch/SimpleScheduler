using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler.Jobs
{
    public class Test2 : IJob
    {
        public void Execute()
        {
            //throw new NotImplementedException();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff")} Job \"{GetType().Name}\" is executed.");
        }
    }
}
