﻿using System;
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

        private readonly IEnumerable<IJobInfo> _listOfJobInfo;

        //private Thread _mainThread;
        private IList<Task> tasks = new List<Task>();

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

        public JobManager(IEnumerable<IJobInfo> listOfJobInfo)
        {
            if (listOfJobInfo != null) _listOfJobInfo = listOfJobInfo;
        }

        public void InitializeAllJobSchedules()
        {
            _log.Debug("Begin Scheduler");

            if (_listOfJobInfo == null || !_listOfJobInfo.Any()) return;

            Parallel.ForEach(_listOfJobInfo.Where(s => s.Enabled), jobInfo =>
            {
                _log.Info(
                        $"Instantiating job \"{jobInfo.Name}\", LogEnabled: {jobInfo.LogEnabled}, Repeatable: {jobInfo.Repeatable}, StopOnError: {jobInfo.StopOnError}, RepetitionIntervalTime: {jobInfo.RepetitionIntervalTime}s, TimeSchedule: {jobInfo.Schedule}.");
                try
                {
                    //_mainThread = new Thread(jobInfo.InitializeSchedule);
                    //if (_mainThread.IsAlive) return;
                    //_mainThread.Start();
                    //await jobInfo.InitializeSchedule();

                    var task = jobInfo.InitializeSchedule();
                    tasks.Add(task);
                    //task.Start();
                }
                catch (Exception ex)
                {
                    _log.Error($"Job \"{jobInfo.Name}\" could not be instantiated or executed.", ex);
                }
            });

            Task.WaitAll(tasks.ToArray());

            _log.Debug("End Scheduler");
        }
    }
}
