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

        public bool LogEnabled { get; }

        public bool Repeatable { get; } = true;

        public bool StopOnError { get; } = true;

        public int RepetitionIntervalTime { get; private set; }

        public string TimeSchedule { get; } = "None";

        private DateTime? _timeSchedule;

        private DateTime? GetTimeSchedule(DateTime now)
        {
            if (string.IsNullOrWhiteSpace(TimeSchedule)) return null;

            if (_timeSchedule != null)
                return new DateTime(now.Year, now.Month, now.Day, _timeSchedule.Value.Hour,
                    _timeSchedule.Value.Minute, _timeSchedule.Value.Second);

            DateTime time;
            if (!DateTime.TryParse(TimeSchedule, out time)) return null;

            _timeSchedule = time;
            if (RepetitionIntervalTime < 1) RepetitionIntervalTime = 1;

            return new DateTime(now.Year, now.Month, now.Day, _timeSchedule.Value.Hour,
                _timeSchedule.Value.Minute, _timeSchedule.Value.Second);
        }

        private string JobType { get; }

        private Type _objectType;

        private Type GetJobObjectType()
        {
            if (string.IsNullOrWhiteSpace(JobType)) return null;

            if (_objectType != null) return _objectType;

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

        public void InitializeSchedule()
        {
            if (!Enabled) return;

            var type = GetJobObjectType();
            if (type == null)
            {
                Log.Error($"Job \"{Name}\" cannot be instantiated.");
                return;
            }

            if (_jobInstance == null)
                _jobInstance = Activator.CreateInstance(type) as IJob;

            while (Repeatable)
            {
                var now = DateTime.Now;

                var timeSchedule = GetTimeSchedule(now);
                if (timeSchedule == null)
                {
                    if (!ExecuteJobAndContinue()) return;
                }
                else
                {
                    var difference = now.TimeOfDay - timeSchedule.Value.TimeOfDay;

                    if (0 <= difference.TotalSeconds && difference.TotalSeconds < RepetitionIntervalTime)
                    {
                        if (!ExecuteJobAndContinue()) return;
                    }
                }

                Thread.Sleep(RepetitionIntervalTime * 1000);
            }

            // else !Repeatable
            ExecuteJobAndContinue();
        }

        private bool ExecuteJobAndContinue()
        {
            try
            {
                Log.Info($"Start job \"{Name}\".");
                _jobInstance.Execute();
            }
            catch (Exception ex)
            {
                if (StopOnError)
                {
                    Log.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                    return false;
                }

                Log.Error($"Job \"{Name}\" could not be executed, throwing an exception.", ex);
            }

            return true;
        }

        public JobInfo() { }

        public JobInfo(string name, bool enabled, bool logEnabled, bool repeatable,
            bool stopOnError, int repetitionIntervalTime, string timeSchedule, string jobType)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (repetitionIntervalTime > 0) RepetitionIntervalTime = repetitionIntervalTime;
            if (!string.IsNullOrWhiteSpace(timeSchedule)) TimeSchedule = timeSchedule;

            Enabled = enabled;
            LogEnabled = logEnabled;
            Repeatable = repeatable;
            StopOnError = stopOnError;
            JobType = jobType;
        }
    }
}
