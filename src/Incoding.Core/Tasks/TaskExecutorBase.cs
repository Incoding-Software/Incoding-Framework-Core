using System;
using System.Threading.Tasks;
using System.Timers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Tasks
{
    public abstract class TaskExecutorBase
    {
        public TaskExecutorOptions Options { get; set; } = new TaskExecutorOptions();
        
        public IDispatcher Dispatcher { get; set; }

        protected TaskExecutorBase()
        {
            Dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
        }

        public TaskExecutorBase SetOptions(Action<TaskExecutorOptions> executorOptions)
        {
            executorOptions?.Invoke(Options);
            return this;
        }

        public static TaskExecutorOptions DefaultOptions = new TaskExecutorOptions
        {
            OnError = exception => LoggingFactory.Instance.LogException(LogType.Debug, exception),
            Interval = TimeSpan.FromSeconds(10),
            DelayToStart = TimeSpan.FromSeconds(30),
            DelayBetweenSequencesMs = 10
        };

        public class TaskExecutorOptions
        {
            private Action<Exception> _onError;
            private TimeSpan? _interval;
            private TimeSpan? _delayToStart;
            private int? _delayBetweenSequencesMs;

            public TimeSpan DelayToStart
            {
                get { return _delayToStart ?? DefaultOptions.DelayToStart; }
                set { _delayToStart = value; }
            }

            public Action<Exception> OnError
            {
                get { return _onError ?? DefaultOptions.OnError; }
                set { _onError = value; }
            }

            public TimeSpan Interval
            {
                get { return _interval ?? DefaultOptions.Interval; }
                set { _interval = value; }
            }

            public Func<bool> Conditional { get; set; } = () => true;

            public int DelayBetweenSequencesMs
            {
                get { return _delayBetweenSequencesMs ?? DefaultOptions.DelayBetweenSequencesMs; }
                set { _delayBetweenSequencesMs = value; }
            }

            public Action AfterExecution { get; set; }
        }

        private Timer _timer;
        private bool _executing;
        private readonly object _lock = new object();
        protected bool StopImmediately = false;
        private DateTime? _lastRunning;
        
        public bool IsExecuting { get { return _executing; } }
        public bool IsEnabled { get { return (_timer?.Enabled).GetValueOrDefault(); } }
        public DateTime? LastRunning
        {
            get { return _lastRunning; }
        }

        protected abstract void Execute();

        public void Start()
        {
            _timer = new Timer(Options.Interval.TotalMilliseconds);
            _timer.Elapsed += (sender, args) =>
            {
                if (_executing)
                    return;
                if (!Options.Conditional())
                    return;
                try
                {
                    lock (_lock)
                    {
                        _executing = true;
                        Execute();
                        Options.AfterExecution?.Invoke();
                        _lastRunning = DateTime.UtcNow;
                        _executing = false;
                    }
                }
                catch (Exception ex)
                {
                    Options.OnError?.Invoke(ex);// LoggingFactory.Instance.Log(LogType.Debug, "TaskManager: ", ex);
                }
            };

            Task.Factory.StartNew(() =>
            {
                Task.Delay(Options.DelayToStart);
                _timer.Start();
            });
        }

        public void Stop(bool immediate)
        {
            if (immediate)
                StopImmediately = true;
            lock (_lock)
            {
                _timer?.Stop();
                _timer?.Dispose();
                _timer = null;
            }
        }
    }
}