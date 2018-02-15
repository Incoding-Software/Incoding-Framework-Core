using Incoding.Core.Block.Logging.Loggers;
using Incoding.Core.Block.Logging.Policy;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Moq;

    #endregion

    public class Context_Logging_Policy
    {
        #region Static Fields

        protected static LoggingPolicy loggingPolicy;

        protected static Mock<ILogger> defaultMockLogger;

        #endregion

        #region Constructors

        protected Context_Logging_Policy()
        {
            defaultMockLogger = new Mock<ILogger>();
        }

        #endregion
    }
}