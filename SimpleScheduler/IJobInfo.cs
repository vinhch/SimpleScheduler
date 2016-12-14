using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        void InitializeSchedule();
    }
}
