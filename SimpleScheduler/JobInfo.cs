using System;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public class JobInfo : IJobInfo
    {
        private const string NONE = "None";

        private IDefaultLog _log;

        public IDefaultLog Log
        {
            get
            {
                return LogEnabled ? _log : null;
            }
            set
            {
                _log = value;
            }
        }

        public string Name { get; } = NONE;

        public bool Enabled { get; private set; }

        public bool LogEnabled { get; }

        public bool Repeatable { get; } = true;

        public bool StopOnError { get; } = true;

        /// <summary>
        /// Time wait after each job task run
        /// If job has Schedule time, then minimum RepetitionIntervalTime is 1ms
        /// </summary>
        public int RepetitionIntervalTime { get; private set; }

        /// <summary>
        /// Schedule time
        /// </summary>
        public string Schedule { get; private set; } = NONE;

        private DateTimeOffset? _timeSchedule;

        private TimeSchedule GetTimeSchedule()
        {
            if (Schedule == NONE || string.IsNullOrWhiteSpace(Schedule)) return null;

            if (_timeSchedule == null)
            {
                if (!DateTimeOffset.TryParse(Schedule, out DateTimeOffset time))
                {
                    Schedule = NONE;
                    return null;
                }

                _timeSchedule = time;
                if (RepetitionIntervalTime < 1) RepetitionIntervalTime = 1; // If job has Schedule time, then minimum RepetitionIntervalTime is 1ms
            }

            var now = DateTimeOffset.Now;
            return new TimeSchedule
            {
                TimeNow = now,
                Value =
                    new DateTime(now.Year, now.Month, now.Day, _timeSchedule.Value.Hour, _timeSchedule.Value.Minute,
                        _timeSchedule.Value.Second)
            };
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

        public async Task InitializeSchedule()
        {
            if (!Enabled) return;

            var type = GetJobObjectType();
            if (type == null)
            {
                Log?.Error($"Job \"{Name}\" cannot be instantiated.");
                return;
            }

            if (_jobInstance == null)
                _jobInstance = Activator.CreateInstance(type) as IJob;

            while (Repeatable)
            {
                var timeSchedule = GetTimeSchedule();
                if (timeSchedule == null)
                {
                    if (!await ExecuteJobAndContinue()) return;
                }
                else
                {
                    var difference = timeSchedule.TimeNow.TimeOfDay - timeSchedule.Value.TimeOfDay;

                    if (0 <= difference.TotalSeconds && difference.TotalSeconds < RepetitionIntervalTime)
                    {
                        if (!await ExecuteJobAndContinue()) return;
                    }
                }

                if (RepetitionIntervalTime > 0)
                {
                    //Thread.Sleep(RepetitionIntervalTime * 1000);
                    await Task.Delay(RepetitionIntervalTime * 1000);
                }
            }

            // else !Repeatable
            await ExecuteJobAndContinue();
        }

        private async Task<bool> ExecuteJobAndContinue()
        {
            Log?.Info($"Start job \"{Name}\".");

            try
            {
                await _jobInstance.Execute();
            }
            catch (Exception ex)
            {
                if (StopOnError)
                {
                    Log?.Error($"Job \"{Name}\" could not be executed, throwing an exception and stopped.", ex);
                    return false;
                }
                Log?.Error($"Job \"{Name}\" could not be executed, throwing an exception.", ex);
            }
            return true;
        }

        public JobInfo() { }

        public JobInfo(string name, bool enabled, bool logEnabled, bool repeatable,
            bool stopOnError, int repetitionIntervalTime, string timeSchedule, string jobType)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (repetitionIntervalTime > 0) RepetitionIntervalTime = repetitionIntervalTime;
            if (!string.IsNullOrWhiteSpace(timeSchedule)) Schedule = timeSchedule;

            Enabled = enabled;
            LogEnabled = logEnabled;
            Repeatable = repeatable;
            StopOnError = stopOnError;
            JobType = jobType;
        }
    }
}
