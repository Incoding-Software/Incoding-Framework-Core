using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Block.Logging.Loggers;
using Incoding.Core.Extensions;

namespace Incoding.Core.Block.Logging.Policy
{
    #region << Using >>

    #endregion

    public interface ILoggingPolicyFor
    {
        ILoggingPolicyUse For(params string[] logTypes);
    }

    public interface ILoggingPolicyUse
    {
        void Use(ILogger logger);

        void UseInLine(Action<string> evaluated);
    }

    public class LoggingPolicy : ILoggingPolicyFor, ILoggingPolicyUse
    {
        #region Fields

        readonly List<ILogger> logContexts = new List<ILogger>();

        string[] supportedTypes;

        #endregion

        #region ILoggingPolicyFor Members

        public ILoggingPolicyUse For(string[] logTypes)
        {
            this.supportedTypes = logTypes;
            return this;
        }


        public ILoggingPolicyUse For(string logType)
        {
            return For(new[] { logType });
        }

        #endregion

        #region ILoggingPolicyUse Members

        public void Use(ILogger logger)
        {
            this.logContexts.Add(logger);
        }

        public void UseInLine(Action<string> evaluated)
        {
            Use(new ActionLogger(evaluated));
        }

        #endregion

        #region Api Methods

        public void Log(string type, LogMessage message)
        {
            if (!this.supportedTypes.Any(r => r.EqualsWithInvariant(type)))
                return;

            foreach (var logger in this.logContexts)
                logger.Log(message);
        }

        public async Task LogAsync(string type, LogMessage message)
        {
            if (!this.supportedTypes.Any(r => r.EqualsWithInvariant(type)))
                return;

            foreach (var logger in this.logContexts)
                await logger.LogAsync(message);
        }

        #endregion
    }
}