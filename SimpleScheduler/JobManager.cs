using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public class JobManager
    {
        private readonly ConcurrentBag<IJobInfo> _listOfJobInfo;

        //private Thread _mainThread;
        private readonly IList<Task> _tasks = new List<Task>();

        public IDefaultLog Log { get; set; }

        public JobManager()
        {
            var schedulerConfigSection = ConfigurationManager.GetSection("schedulerConfig");

            var configSection = schedulerConfigSection as SchedulerConfigSection;
            if (configSection != null)
            {
                _listOfJobInfo = new ConcurrentBag<IJobInfo>(configSection.Jobs.Select(jobConfig =>
                {
                    var jobInfo = new JobInfo(jobConfig.Name, jobConfig.Enabled, jobConfig.Logging, true,
                        jobConfig.StopOnError, jobConfig.Seconds, jobConfig.Schedule,
                        jobConfig.Type);
                    jobInfo.Log = Log;

                    return jobInfo;
                }));
            }
        }

        public JobManager(IEnumerable<IJobInfo> listOfJobInfo)
        {
            if (listOfJobInfo != null) _listOfJobInfo = new ConcurrentBag<IJobInfo>(listOfJobInfo);
        }

        public void InitializeAllJobSchedules()
        {
            Log?.Debug("Begin Scheduler");

            if (_listOfJobInfo == null || !_listOfJobInfo.Any()) return;

            Parallel.ForEach(_listOfJobInfo.Where(s => s.Enabled), jobInfo =>
            {
                Log?.Info(
                    $"Instantiating job \"{jobInfo.Name}\"," +
                    $" LogEnabled: {jobInfo.LogEnabled}," +
                    $" Repeatable: {jobInfo.Repeatable}," +
                    $" StopOnError: {jobInfo.StopOnError}," +
                    $" RepetitionIntervalTime: {jobInfo.RepetitionIntervalTime}s," +
                    $" TimeSchedule: {jobInfo.Schedule}.");
                try
                {
                    var task = jobInfo.InitializeSchedule();
                    _tasks.Add(task);
                    //task.Start();
                }
                catch (Exception ex)
                {
                    Log?.Error($"Job \"{jobInfo.Name}\" could not be instantiated or executed.", ex);
                }
            });

            Task.WaitAll(_tasks.ToArray());
        }
    }
}
