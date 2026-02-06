#region copyright

// @incoding 2011
#endregion

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging.Core;
using static System.Net.Mime.MediaTypeNames;

namespace Incoding.Core.Block.Logging.Loggers
{
    #region << Using >>

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for stream file writer
    /// </summary>
    public class FileLogger : LoggerBase
    {
        ////ncrunch: no coverage start
        #region Static Fields

        static readonly object lockObject = new object();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        #endregion

        ////ncrunch: no coverage end
        #region Fields

        readonly bool append;

        readonly string folderPath;

        readonly Func<string> fileName;

        bool clearAtOnce;

        #endregion

        #region Constructors

        FileLogger(string folderPath, Func<string> fileName, bool append, bool clearAtOnce)
        {
            Guard.NotNullOrWhiteSpace("folderPath", folderPath);
            Guard.NotNull("fileName", fileName);

            this.folderPath = folderPath;
            this.fileName = fileName;
            this.append = append;
            this.clearAtOnce = clearAtOnce;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        #endregion

        #region Factory constructors

        public static FileLogger WithAtOnceReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, true, true);
        }

        public static FileLogger WithEachReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, false, true);
        }

        public static FileLogger WithoutReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, true, false);
        }

        #endregion

        public override void Log(LogMessage logMessage)
        {
            lock (lockObject)
            {
                string fullPath = Path.Combine(this.folderPath, this.fileName.Invoke());
                if (File.Exists(fullPath))
                {
                    if (this.clearAtOnce)
                        File.Delete(fullPath);
                }

                this.clearAtOnce = false;

                using (var streamWriter = new StreamWriter(@fullPath, this.append))
                {
                    string message = this.template.Invoke(logMessage);
                    streamWriter.Write(message);
                    streamWriter.Flush();
                }
            }
        }

        /// <summary>
        /// Log Async
        /// </summary>
        /// <param name="logMessage">Message to log</param>
        public override async Task LogAsync(LogMessage logMessage)
        {
            string fullPath = Path.Combine(this.folderPath, this.fileName.Invoke());
            lock (lockObject)
            {
                if (this.clearAtOnce)
                {
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);

                    this.clearAtOnce = false;
                }
            }

            string message = this.template.Invoke(logMessage);
            await _semaphore.WaitAsync(int.MaxValue);
            try
            {
                using (var streamWriter = new StreamWriter(@fullPath, this.append))
                {
                    await streamWriter.WriteAsync(message);
                    await streamWriter.FlushAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}