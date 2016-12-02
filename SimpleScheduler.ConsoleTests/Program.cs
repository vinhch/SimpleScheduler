using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler.ConsoleTests
{
    class Program
    {
        private static readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _logger.Info($"Running as {WindowsIdentity.GetCurrent().Name}");

            var jobManager = new JobManager();
            jobManager.ExecuteAllJobs();
        }
    }
}
