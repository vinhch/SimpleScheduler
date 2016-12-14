using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler.Tests
{
    public class JobManagerTest
    {
        [SetUp]
        public void BeforeTest()
        {
        }

        [Test]
        public void Create_JobManager_With_Ctor()
        {
            var jobs = new List<IJobInfo>
            {
                new JobInfo(),
                new JobInfo(),
            };

            var jobManager = new JobManager(jobs);
        }

        [TearDown]
        public void AfterTest()
        {
        }
    }
}
