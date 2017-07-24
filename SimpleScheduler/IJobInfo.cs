using System;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public interface IJobInfo
    {
        string Name { get; }
        bool Enabled { get; }
        bool LogEnabled { get; }
        bool Repeatable { get; }
        bool StopOnError { get; }
        int RepetitionIntervalTime { get; }
        string Schedule { get; }
        DateTimeOffset LastRunAt { get; }
        bool IsSelfExecute { get; }
        Task InitializeSchedule();
    }
}
