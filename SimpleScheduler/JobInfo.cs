using System;
using System.Threading;
using log4net;

namespace SimpleScheduler
{
    public class JobInfo
    {
        private readonly ILog _log = LogManager.GetLogger(Constants.SCHEDULER_LOGGER);

        private ILog Log
        {
            get
            {
                if (LogEnabled) return _log;
                return NullLog.GetNullLog();
            }
        }

        public string Name { get; } = "Unknow";

        public bool Enabled { get; private set; }

        public bool LogEnabled { get; private set; }

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

        private IJob _jobInstance;

        public void ExecuteJob()
        {
            if (!Enabled) return;

            var type = GetJobObjectType();

            if (type == null)
            {
                Log.Error($"Job \"{Name}\" cannot be instantiated.");
                return;
            }

            if (_jobInstance == null)
                _jobInstance = Activator.CreateInstance(GetJobObjectType()) as IJob;

            if (Repeatable)
            {
                while (true)
                {
                    Log.Info($"Start job \"{Name}\".");

                    try
                    {
                        _jobInstance.Execute();
                    }
                    catch (Exception ex)
                    {
                        if (StopOnError)
                        {
                            Log.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                            return;
                        }

                        Log.Error($"Job \"{Name}\" could not be executed, throwing an exception.", ex);
                    }

                    Thread.Sleep(RepetitionIntervalTime * 1000);
                }
            }
            else
            {
                try
                {
                    _jobInstance.Execute();
                }
                catch (Exception ex)
                {
                    Log.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                }
            }
        }

        public JobInfo() { }

        public JobInfo(string name, bool enabled, bool logEnabled, bool repeatable,
            bool stopOnError, int repetitionIntervalTime, string jobType)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (repetitionIntervalTime > 0) RepetitionIntervalTime = repetitionIntervalTime;

            Enabled = enabled;
            LogEnabled = logEnabled;
            Repeatable = repeatable;
            StopOnError = stopOnError;
            JobType = jobType;
        }
    }
}
