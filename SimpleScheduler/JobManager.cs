using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public class JobManager
    {
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

            if (_listOfJobInfo == null || !_listOfJobInfo.Any()) return;

            Parallel.ForEach(_listOfJobInfo.Where(s => s.Enabled), jobInfo =>
            {
                try
                {
                    var task = jobInfo.InitializeSchedule();
                    tasks.Add(task);
                    //task.Start();
                }
                catch (Exception)
                {
                }
            });

            Task.WaitAll(tasks.ToArray());
        }
    }
}
