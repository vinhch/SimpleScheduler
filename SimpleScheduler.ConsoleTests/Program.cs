using log4net;
using System;
using System.Security.Principal;

namespace SimpleScheduler.ConsoleTests
{
    class Program
    {
        private static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _log.Info($"Running as {WindowsIdentity.GetCurrent().Name}");

            var jobManager = new JobManager();
            jobManager.InitializeAllJobSchedules();

            //Console.ReadLine();
        }
    }
}
