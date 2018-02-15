#region copyright

// @incoding 2011
#endregion

using System;
using Incoding.Core.Block.Logging.Core;

namespace Incoding.Core.Block.Logging.Loggers
{
    #region << Using >>

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for console.
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        public override void Log(LogMessage logMessage)
        {
            Console.Write(this.template(logMessage));
        }
    }
}