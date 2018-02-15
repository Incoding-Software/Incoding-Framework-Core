using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Loggers;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Moq;

    #endregion

    public class Context_logging_factory
    {
        #region Static Fields

        protected static Mock<ILogger> defaultMockLogger;

        protected static LoggingFactory loggingFactory;

        #endregion

        #region Constructors

        protected Context_logging_factory()
        {
            defaultMockLogger = new Mock<ILogger>();
            loggingFactory = new LoggingFactory();
        }

        #endregion
    }
}