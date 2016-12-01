using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace SimpleScheduler
{
    public class JobManager
    {
        private readonly ILog _logger = LogManager.GetLogger(Constants.SCHEDULER_LOGGER);

        private IEnumerable<JobInfo> _listOfJobInfo;

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
                            new JobInfo(jobConfig.Name, jobConfig.Enabled, true, jobConfig.StopOnError,
                                jobConfig.Seconds, jobConfig.Type));

            }
        }

        public JobManager(IEnumerable<JobInfo> listOfJobInfo)
        {
            if (listOfJobInfo != null) _listOfJobInfo = listOfJobInfo;
        }

        public void ExecuteAllJobs()
        {
            _logger.Debug("Begin Scheduler");

            if (_listOfJobInfo == null || !_listOfJobInfo.Any())
            {
                return;
            }

            //foreach (var jobInfo in _listOfJobInfo)
            //{
            //    var jobThread = new Thread(new ThreadStart(jobInfo.ExecuteJob));
            //}

            Parallel.ForEach(_listOfJobInfo.Where(s => s.Enabled), jobInfo =>
            {
                try
                {
                    _logger.Info($"The Job \"{jobInfo.Name}\".");

                    _mainThread = new Thread(jobInfo.ExecuteJob);

                    if (!_mainThread.IsAlive)
                    {
                        _mainThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"The Job \"{jobInfo.Name}\" could not be instantiated or executed.", ex);
                }
            });

            _logger.Debug("End Scheduler");
        }

    }
}
