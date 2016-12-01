using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public class JobManager
    {
        private IEnumerable<JobInfo> _listOfJobInfo;
        public JobManager()
        {

        }

        public void ExecuteAllJobs()
        {
            if (_listOfJobInfo == null || _listOfJobInfo.Count() <= 0)
            {
                return;
            }

            foreach (var jobInfo in _listOfJobInfo)
            {
                var jobThread = new Thread(new ThreadStart(jobInfo.ExecuteJob));
            }
        }


    }
}
