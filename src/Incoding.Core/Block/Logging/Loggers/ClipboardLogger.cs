using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Utilities;

namespace Incoding.Core.Block.Logging.Loggers
{
    public class ClipboardLogger : LoggerBase
    {
        public override void Log(LogMessage logMessage)
        {
            Clipboard.Copy(this.template(logMessage));
        }
    }
}