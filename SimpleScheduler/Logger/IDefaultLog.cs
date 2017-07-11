using System;

namespace SimpleScheduler
{
    public interface IDefaultLog
    {
        void SetLogDebug(Action<string> executeLogCallBack);

        void Debug(string message);

        void SetLogInfo(Action<string> executeLogCallBack);

        void Info(string message);

        void SetLogError(Action<string, Exception> executeLogCallBack);

        void Error(string message, Exception ex = null);
    }
}
