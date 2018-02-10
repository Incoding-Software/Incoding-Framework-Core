using Incoding.Core.Utilities;

namespace Incoding.Block.Logging
{
    public class ClipboardLogger : LoggerBase
    {
        public override void Log(LogMessage logMessage)
        {
            Clipboard.Copy(this.template(logMessage));
        }
    }
}