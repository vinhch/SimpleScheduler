using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobManager = new JobManager();
            jobManager.ExecuteAllJobs();
        }
    }
}
