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

            var jobManager = new JobManager
            {
                //Log = CreateDefaultLog()
            };
            jobManager.InitializeAllJobSchedules();

            //Console.ReadLine();
        }

        private static IDefaultLog CreateDefaultLog()
        {
            var logForScheduler = LogManager.GetLogger(Constants.SCHEDULER_LOGGER);

            var log = new DefaultLog();
            log.SetLogDebug(msg => logForScheduler.Debug(msg));
            log.SetLogInfo(msg => logForScheduler.Info(msg));
            log.SetLogError((msg, ex) => logForScheduler.Error(msg, ex));

            return log;
        }
    }
}
