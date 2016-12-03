using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace SimpleScheduler
{
    public class JobManager
    {
        private readonly ILog _log = LogManager.GetLogger(Constants.SCHEDULER_LOGGER);

        private readonly IEnumerable<JobInfo> _listOfJobInfo;

        private Thread _mainThread;

        public JobManager()
        {
            var schedulerConfigSection = ConfigurationManager.GetSection("schedulerConfig");

            var configSection = schedulerConfigSection as SchedulerConfigSection;
            if (configSection != null)
            {
                _listOfJobInfo =
                    configSection.Jobs.Select(
                        jobConfig =>
                            new JobInfo(jobConfig.Name, jobConfig.Enabled, jobConfig.Logging, true,
                                jobConfig.StopOnError, jobConfig.Seconds, jobConfig.Schedule,
                                jobConfig.Type));

            }
        }

        public JobManager(IEnumerable<JobInfo> listOfJobInfo)
        {
            if (listOfJobInfo != null) _listOfJobInfo = listOfJobInfo;
        }

        public void ExecuteAllJobs()
        {
            _log.Debug("Begin Scheduler");

            if (_listOfJobInfo == null || !_listOfJobInfo.Any())
            {
                return;
            }

            Parallel.ForEach(_listOfJobInfo.Where(s => s.Enabled), jobInfo =>
            {
                try
                {
                    _log.Info(
                        $"Instantiating job \"{jobInfo.Name}\", LogEnabled: {jobInfo.LogEnabled}, Repeatable: {jobInfo.Repeatable}, StopOnError: {jobInfo.StopOnError}, RepetitionIntervalTime: {jobInfo.RepetitionIntervalTime}s, TimeSchedule: {jobInfo.TimeSchedule}.");

                    _mainThread = new Thread(jobInfo.ExecuteJob);

                    if (!_mainThread.IsAlive)
                    {
                        _mainThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"Job \"{jobInfo.Name}\" could not be instantiated or executed.", ex);
                }
            });

            _log.Debug("End Scheduler");
        }
    }
}
