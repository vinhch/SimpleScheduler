using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace SimpleScheduler
{
    public class JobInfo
    {
        private readonly ILog _logger = LogManager.GetLogger(Constants.SCHEDULER_LOGGER);

        public string Name { get; } = "Unknow";

        public bool Enabled { get; private set; }

        public bool Repeatable { get; } = true;

        public bool StopOnError { get; } = true;

        public int RepetitionIntervalTime { get; }

        public string JobType { get; }

        private Type _objectType;

        public Type GetJobObjectType()
        {
            if (_objectType != null) return _objectType;

            if (string.IsNullOrWhiteSpace(JobType)) return null;

            //var type = Type.GetType($"{Name}, SimpleScheduler.Jobs");
            var type = Type.GetType(JobType);

            if (!IsRealJobClass(type))
            {
                Enabled = false;
                return null;
            }

            _objectType = type;
            return _objectType;
        }

        private static bool IsRealJobClass(Type testType)
        {
            return testType != null
                && testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false
                && testType.GetInterface(nameof(IJob)) != null;
        }

        private IJob _instanceJob;

        public void ExecuteJob()
        {
            if (!Enabled) return;

            var type = GetJobObjectType();

            if (type == null)
            {
                _logger.Error($"Job \"{Name}\" cannot be instantiated.");
                return;
            }

            if (_instanceJob == null)
                _instanceJob = Activator.CreateInstance(GetJobObjectType()) as IJob;

            if (Repeatable)
            {
                while (true)
                {
                    _logger.Info($"Start job \"{Name}\".");

                    try
                    {
                        _instanceJob.Execute();
                    }
                    catch (Exception ex)
                    {
                        if (StopOnError)
                        {
                            _logger.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                            return;
                        }

                        _logger.Error($"Job \"{Name}\" could not be executed, throwing an exception.", ex);
                    }

                    Thread.Sleep(RepetitionIntervalTime*1000);
                }
            }
            else
            {
                try
                {
                    _instanceJob.Execute();
                }
                catch (Exception ex)
                {
                    _logger.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                }
            }
        }

        public JobInfo() { }

        public JobInfo(string name, bool enabled, bool repeatable,
            bool stopOnError, int repetitionIntervalTime, string jobType)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (repetitionIntervalTime > 0) RepetitionIntervalTime = repetitionIntervalTime;

            Enabled = enabled;
            Repeatable = repeatable;
            StopOnError = stopOnError;
            JobType = jobType;
        }
    }
}
