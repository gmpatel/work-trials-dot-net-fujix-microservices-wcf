using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core
{
    public interface ILogger
    {
        void Debug(object message, Exception exception);
        void Debug(object message);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        void DebugFormat(string format, object arg0, object arg1, object arg2);
        void DebugFormat(string format, object arg0, object arg1);
        void DebugFormat(string format, object arg0);
        void DebugFormat(string format, params object[] args);
        void Error(object message, Exception exception);
        void Error(object message);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(string format, object arg0, object arg1, object arg2);
        void ErrorFormat(string format, object arg0, object arg1);
        void ErrorFormat(string format, object arg0);
        void ErrorFormat(string format, params object[] args);
        void Fatal(object message, Exception exception);
        void Fatal(object message);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
        void FatalFormat(string format, object arg0, object arg1, object arg2);
        void FatalFormat(string format, object arg0, object arg1);
        void FatalFormat(string format, object arg0);
        void FatalFormat(string format, params object[] args);
        void Info(object message, Exception exception);
        void Info(object message);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(string format, object arg0, object arg1, object arg2);
        void InfoFormat(string format, object arg0, object arg1);
        void InfoFormat(string format, object arg0);
        void InfoFormat(string format, params object[] args);
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        void Warn(object message, Exception exception);
        void Warn(object message);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(string format, object arg0, object arg1, object arg2);
        void WarnFormat(string format, object arg0, object arg1);
        void WarnFormat(string format, object arg0);
        void WarnFormat(string format, params object[] args);
    }
}