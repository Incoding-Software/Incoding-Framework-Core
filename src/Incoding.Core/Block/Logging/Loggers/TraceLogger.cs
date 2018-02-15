using System.Diagnostics;

namespace Incoding.Core.Block.Logging.Loggers
{
    #region << Using >>

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for <see cref="Trace" />
    /// </summary>
    public class TraceLogger : ActionLogger
    {
        #region Constructors

        public TraceLogger(string category)
                : base(context => Trace.Write(context, category)) { }

        #endregion
    }
}