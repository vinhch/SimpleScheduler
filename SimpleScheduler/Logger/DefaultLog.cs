using System;

namespace SimpleScheduler
{
    public class DefaultLog : IDefaultLog
    {
        private Action<string> _executeLogDebug;
        public void SetLogDebug(Action<string> executeLogCallBack)
        {
            _executeLogDebug = executeLogCallBack;
        }

        public void Debug(string message)
        {
            _executeLogDebug?.Invoke(message);
        }

        private Action<string> _executeLogInfo;

        public void SetLogInfo(Action<string> executeLogCallBack)
        {
            _executeLogInfo = executeLogCallBack;
        }

        public void Info(string message)
        {
            _executeLogInfo?.Invoke(message);
        }

        private Action<string, Exception> _executeLogError;

        public void SetLogError(Action<string, Exception> executeLogCallBack)
        {
            _executeLogError = executeLogCallBack;
        }

        public void Error(string message, Exception ex = null)
        {
            _executeLogError?.Invoke(message, ex);
        }
    }
}
