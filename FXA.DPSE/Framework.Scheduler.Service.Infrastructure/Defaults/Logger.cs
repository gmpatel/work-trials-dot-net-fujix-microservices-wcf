using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Core;
using log4net;

namespace FXA.DPSE.Framework.Scheduler.Service.Infrastructure.Defaults
{
    public class Logger : ILogger
    {
        private readonly bool _consoleDebugWriteEnabled;
        private readonly bool _consoleInfoWriteEnabled;
        private readonly bool _consoleErrorWriteEnabled;
        private readonly bool _consoleFatalWriteEnabled;
        private readonly bool _consoleWarningWriteEnabled;

        private static volatile ILogger _instance;
        private static ILog _logger = null;

        private static readonly object ConsturctorLock = new Object();

        private readonly object _syncLock = new Object();

        private Logger()
        {
            _consoleDebugWriteEnabled = true;
            _consoleInfoWriteEnabled = true;
            _consoleErrorWriteEnabled = true;
            _consoleFatalWriteEnabled = true;
            _consoleWarningWriteEnabled = true;

            if (_logger == null)
            {
                _logger =
                    LogManager
                    .GetLogger("Log4Net");

                log4net
                    .Config
                    .XmlConfigurator
                    .Configure();
            }
        }

        public static ILogger Instance()
        {
            if (_instance == null)
            {
                lock (ConsturctorLock)
                {
                    if (_instance == null) _instance = new Logger();
                }
            }

            return _instance;
        }

        private static string GetCallerInfo()
        {
            var stackFrames = new StackTrace().GetFrames();

            if (stackFrames != null)
            {
                var method = stackFrames[2].GetMethod();

                if (method.DeclaringType != null)
                {
                    //, Full Name = {2}, method.DeclaringType.FullName
                    return string.Format(">> Method = {0}(), Class Name = {1} <<", method.Name, method.DeclaringType.Name);
                }
            }

            return string.Empty;
        }

        public void Debug(object message, Exception exception)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.Debug(string.Format("{0} {1}, Exception Message : {2}", GetCallerInfo(), message, exception.Message), exception);
                    if (_consoleDebugWriteEnabled) Console.WriteLine("Debug : {0}, Exception Message : {1}", message, exception.Message);
                }
            }
        }

        public void Debug(object message)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.Debug(string.Format("{0} {1}", GetCallerInfo(), message));
                    if (_consoleDebugWriteEnabled) Console.WriteLine("Debug : {0}", message);
                }
            }
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.DebugFormat(provider, format, args);
                    if (_consoleDebugWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.DebugFormat(format, arg0, arg1, arg2);
                    if (_consoleDebugWriteEnabled) Console.WriteLine(format, arg0, arg1, arg2);
                }
            }
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.DebugFormat(format, arg0, arg1);
                    if (_consoleDebugWriteEnabled) Console.WriteLine(format, arg0, arg1);
                }
            }
        }

        public void DebugFormat(string format, object arg0)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.DebugFormat(format, arg0);
                    if (_consoleDebugWriteEnabled) Console.WriteLine(format, arg0);
                }
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsDebugEnabled)
                {
                    _logger.DebugFormat(format, args);
                    if (_consoleDebugWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void Error(object message, Exception exception)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.Error(string.Format("{0} {1}, Exception Message : {2}", GetCallerInfo(), message, exception.Message), exception);
                    if (_consoleErrorWriteEnabled) Console.WriteLine("Error : {0}, Exception Message : {1}", message, exception.Message);
                }
            }
        }

        public void Error(object message)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.Error(string.Format("{0} {1}", GetCallerInfo(), message));
                    if (_consoleErrorWriteEnabled) Console.WriteLine("Error : {0}", message);
                }

            }
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.ErrorFormat(provider, format, args);
                    if (_consoleErrorWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.ErrorFormat(format, arg0, arg1, arg2);
                    if (_consoleErrorWriteEnabled) Console.WriteLine(format, arg0, arg1, arg2);
                }
            }
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.ErrorFormat(format, arg0, arg1);
                    if (_consoleErrorWriteEnabled) Console.WriteLine(format, arg0, arg1);
                }
            }
        }

        public void ErrorFormat(string format, object arg0)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.ErrorFormat(format, arg0);
                    if (_consoleErrorWriteEnabled) Console.WriteLine(format, arg0);
                }
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsErrorEnabled)
                {
                    _logger.ErrorFormat(format, args);
                    if (_consoleErrorWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void Fatal(object message, Exception exception)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.Fatal(string.Format("{0} {1}, Exception Message : {2}", GetCallerInfo(), message, exception.Message), exception);
                    if (_consoleFatalWriteEnabled) Console.WriteLine("Fatal : {0}, Exception Message : {1}", message, exception.Message);
                }
            }
        }

        public void Fatal(object message)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.Fatal(string.Format("{0} {1}", GetCallerInfo(), message));
                    if (_consoleFatalWriteEnabled) Console.WriteLine("Fatal : {0}", message);
                }
            }
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.FatalFormat(provider, format, args);
                    if (_consoleFatalWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.FatalFormat(format, arg0, arg1, arg2);
                    if (_consoleFatalWriteEnabled) Console.WriteLine(format, arg0, arg1, arg2);
                }
            }
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.FatalFormat(format, arg0, arg1);
                    if (_consoleFatalWriteEnabled) Console.WriteLine(format, arg0, arg1);
                }
            }
        }

        public void FatalFormat(string format, object arg0)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.FatalFormat(format, arg0);
                    if (_consoleFatalWriteEnabled) Console.WriteLine(format, arg0);
                }
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsFatalEnabled)
                {
                    _logger.FatalFormat(format, args);
                    if (_consoleFatalWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void Info(object message, Exception exception)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.Info(string.Format("{0} {1}, Exception Message : {2}", GetCallerInfo(), message, exception.Message), exception);
                    if (_consoleInfoWriteEnabled) Console.WriteLine("Info : {0}, Exception Message : {1}", message, exception.Message);
                }
            }
        }

        public void Info(object message)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.Info(string.Format("{0} {1}", GetCallerInfo(), message));
                    if (_consoleInfoWriteEnabled) Console.WriteLine("Info : {0}", message);
                }
            }
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.InfoFormat(provider, format, args);
                    if (_consoleInfoWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.InfoFormat(format, arg0, arg1, arg2);
                    if (_consoleInfoWriteEnabled) Console.WriteLine(format, arg0, arg1, arg2);
                }
            }
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.InfoFormat(format, arg0, arg1);
                    if (_consoleInfoWriteEnabled) Console.WriteLine(format, arg0, arg1);
                }
            }
        }

        public void InfoFormat(string format, object arg0)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.InfoFormat(format, arg0);
                    if (_consoleInfoWriteEnabled) Console.WriteLine(format, arg0);
                }
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsInfoEnabled)
                {
                    _logger.InfoFormat(format, args);
                    if (_consoleInfoWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }

        public void Warn(object message, Exception exception)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.Warn(string.Format("{0} {1}, Exception Message : {2}", GetCallerInfo(), message, exception.Message), exception);
                    if (_consoleWarningWriteEnabled) Console.WriteLine("Warn : {0}, Exception Message : {1}", message, exception.Message);
                }
            }
        }

        public void Warn(object message)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.Warn(string.Format("{0} {1}", GetCallerInfo(), message));
                    if (_consoleWarningWriteEnabled) Console.WriteLine("Warn : {0}", message);
                }
            }
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.WarnFormat(provider, format, args);
                    if (_consoleWarningWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.WarnFormat(format, arg0, arg1, arg2);
                    if (_consoleWarningWriteEnabled) Console.WriteLine(format, arg0, arg1, arg2);
                }
            }
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.WarnFormat(format, arg0, arg1);
                    if (_consoleWarningWriteEnabled) Console.WriteLine(format, arg0, arg1);
                }
            }
        }

        public void WarnFormat(string format, object arg0)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.WarnFormat(format, arg0);
                    if (_consoleWarningWriteEnabled) Console.WriteLine(format, arg0);
                }
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            lock (_syncLock)
            {
                if (IsWarnEnabled)
                {
                    _logger.WarnFormat(format, args);
                    if (_consoleWarningWriteEnabled) Console.WriteLine(format, args);
                }
            }
        }
    }
}