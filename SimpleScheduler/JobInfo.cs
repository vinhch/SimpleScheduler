using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public class JobInfo
    {
        public string Name { get; }

        public bool Enabled { get; } = false;

        public bool Repeatable { get; } = true;

        public int RepetitionIntervalTime { get; }

        public string JobType { get; }

        private Type _objectType;

        public Type GetJobObjectType()
        {
            if (_objectType != null) return _objectType;

            if (string.IsNullOrWhiteSpace(JobType)) return null;

            //var type = Type.GetType($"{Name}, SimpleScheduler.Jobs");
            var type = Type.GetType(JobType);
            if (!IsRealJobClass(type)) return null;

            _objectType = type;
            return _objectType;
        }

        public static bool IsRealJobClass(Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false
                && testType.IsAssignableFrom(typeof(IJob));
        }

        private IJob _instanceJob;

        public void ExecuteJob()
        {
            if (_instanceJob == null)
                _instanceJob = Activator.CreateInstance(GetJobObjectType()) as IJob;

            if (Repeatable)
            {
                while (true)
                {
                    _instanceJob.Execute();
                    Thread.Sleep(RepetitionIntervalTime);
                }
            }
            else
            {
                _instanceJob.Execute();
            }
        }
    }
}
