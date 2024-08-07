﻿using System;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Extensions;

namespace Incoding.Core.Block.Logging.Loggers
{
    #region << Using >>

    #endregion

    public abstract class LoggerBase : ILogger
    {
        #region Fields

        protected Func<LogMessage, string> template;

        #endregion

        #region Constructors

        protected LoggerBase()
        {
            WithTemplate(message =>
                             {
                                 var dt = DateTime.Now;
                                 var res = new StringBuilder();                                 
                                 if (!string.IsNullOrWhiteSpace(message.Message))
                                     res.AppendLine("Message by {0}:{1}".F(dt, message.Message));

                                 if (message.Exception != null)
                                     res.AppendLine("Exception by {0}:{1}".F(dt, message.Exception));

                                 return res.ToString();
                             });
        }

        #endregion

        #region ILogger Members

        public abstract void Log(LogMessage logMessage);
        public abstract Task LogAsync(LogMessage logMessage);
        
        public ILogger WithTemplate(Func<LogMessage, string> func)
        {
            this.template = func;
            return this;
        }

        #endregion
    }
}