using JetBrains.Annotations;

namespace Incoding.Core.Block.Logging.Core
{
    #region << Using >>

    #endregion

    public class LogType
    {
        #region Constants

        public const string Debug = "Debug";

        [UsedImplicitly]
        public const string Release = "Release";

        public const string Trace = "Trace";

        [UsedImplicitly]
        public const string Fatal = "Fatal";

        [UsedImplicitly]
        public const string Info = "Info";

        #endregion
    }
}