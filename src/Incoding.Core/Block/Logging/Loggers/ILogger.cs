using System;
using Incoding.Core.Block.Logging.Core;

namespace Incoding.Core.Block.Logging.Loggers
{
    #region << Using >>

    #endregion

    public interface ILogger
    {
        void Log(LogMessage logMessage);

        ILogger WithTemplate(Func<LogMessage, string> func);
    }
}