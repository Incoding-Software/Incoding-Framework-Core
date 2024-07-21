using System;
using System.Threading.Tasks;
using Incoding.Core.Block.Core;
using Incoding.Core.Block.Logging.Core;

namespace Incoding.Core.Block.Logging
{
    #region << Using >>

    #endregion

    public class LoggingFactory : FactoryBase<InitLogging>
    { 
        #region Static Fields

        static readonly Lazy<LoggingFactory> instance = new Lazy<LoggingFactory>(() => new LoggingFactory());

        #endregion

        #region Properties

        public static LoggingFactory Instance { get { return instance.Value; } }

        #endregion

        #region Api Methods

        public void LogException(string logType, Exception exception)
        {
            string message = this.init.parser.Parse(exception);
            ExecuteLog(logType, new LogMessage(message, exception, null));
        }

        public async Task LogExceptionAsync(string logType, Exception exception)
        {
            string message = this.init.parser.Parse(exception);
            await ExecuteLogAsync(logType, new LogMessage(message, exception, null));
        }

        public void LogMessage(string logType, string message)
        {
            ExecuteLog(logType, new LogMessage(message, null, null));
        }

        public void Log(string logType, string message, Exception exception, object state = null)
        {
            ExecuteLog(logType, new LogMessage(message, exception, state));
        }

        #endregion

        void ExecuteLog(string logType, LogMessage message)
        {
            foreach (var policy in this.init.policies)
                policy.Log(logType, message);
        }
        async Task ExecuteLogAsync(string logType, LogMessage message)
        {
            foreach (var policy in this.init.policies)
                await policy.LogAsync(logType, message);
        }
    }
}