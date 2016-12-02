﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace SimpleScheduler
{
    // using Null Object pattern
    public class NullLog : ILog
    {
        private static NullLog _nullLogInstance;
        public static NullLog GetNullLog()
        {
            if (_nullLogInstance == null)
                _nullLogInstance = new NullLog();

            return _nullLogInstance;
        }

        #region Implement empty properties and methods
        public bool IsDebugEnabled { get; }

        public bool IsErrorEnabled { get; }

        public bool IsFatalEnabled { get; }

        public bool IsInfoEnabled { get; }

        public bool IsWarnEnabled { get; }

        public ILogger Logger { get; }

        public void Debug(object message) { }

        public void Debug(object message, Exception exception) { }

        public void DebugFormat(string format, object arg0) { }

        public void DebugFormat(string format, params object[] args) { }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args) { }

        public void DebugFormat(string format, object arg0, object arg1) { }

        public void DebugFormat(string format, object arg0, object arg1, object arg2) { }

        public void Error(object message) { }

        public void Error(object message, Exception exception) { }

        public void ErrorFormat(string format, object arg0) { }

        public void ErrorFormat(string format, params object[] args) { }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { }

        public void ErrorFormat(string format, object arg0, object arg1) { }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { }

        public void Fatal(object message) { }

        public void Fatal(object message, Exception exception) { }

        public void FatalFormat(string format, object arg0) { }

        public void FatalFormat(string format, params object[] args) { }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { }

        public void FatalFormat(string format, object arg0, object arg1) { }

        public void FatalFormat(string format, object arg0, object arg1, object arg2) { }

        public void Info(object message) { }

        public void Info(object message, Exception exception) { }

        public void InfoFormat(string format, object arg0) { }

        public void InfoFormat(string format, params object[] args) { }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { }

        public void InfoFormat(string format, object arg0, object arg1) { }

        public void InfoFormat(string format, object arg0, object arg1, object arg2) { }

        public void Warn(object message) { }

        public void Warn(object message, Exception exception) { }

        public void WarnFormat(string format, object arg0) { }

        public void WarnFormat(string format, params object[] args) { }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { }

        public void WarnFormat(string format, object arg0, object arg1) { }

        public void WarnFormat(string format, object arg0, object arg1, object arg2) { }
        #endregion
    }

}
