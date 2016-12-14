using NUnit.Framework;
using SimpleScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler.Tests
{
    class JobInfoTests
    {
        static string NONE = "None";
        IJobInfo job;

        [SetUp]
        public void BeforeTest()
        {
        }

        [Test]
        public void Create_Job_With_Default_Ctor()
        {
            job = new JobInfo();

            Assert.IsFalse(job.Enabled);
            Assert.AreEqual(NONE, job.Name);
            Assert.AreEqual(NONE, job.Schedule);
            Assert.IsFalse(job.LogEnabled);

            Assert.IsTrue(job.Repeatable);
            Assert.IsTrue(job.StopOnError);
            Assert.AreEqual(0, job.RepetitionIntervalTime);
        }

        [Test]
        public void Create_Job_With_Ctor()
        {
            var TEST1 = "test1";
            job = new JobInfo(TEST1, true, true, true, true, 1, TEST1, TEST1);

            Assert.AreEqual(TEST1, job.Name);
            Assert.AreEqual(TEST1, job.Schedule);
            Assert.AreEqual(1, job.RepetitionIntervalTime);

            Assert.IsTrue(job.Enabled);
            Assert.IsTrue(job.LogEnabled);
            Assert.IsTrue(job.Repeatable);
            Assert.IsTrue(job.StopOnError);

            var TEST2 = "test2";
            job = new JobInfo(TEST2, false, false, false, false, 2, TEST2, TEST2);

            Assert.AreEqual(TEST2, job.Name);
            Assert.AreEqual(TEST2, job.Schedule);
            Assert.AreEqual(2, job.RepetitionIntervalTime);

            Assert.IsFalse(job.Enabled);
            Assert.IsFalse(job.LogEnabled);
            Assert.IsFalse(job.Repeatable);
            Assert.IsFalse(job.StopOnError);
        }

        [TearDown]
        public void AfterTest()
        {

        }
    }
}
