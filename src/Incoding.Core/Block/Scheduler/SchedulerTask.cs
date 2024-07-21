using System;
using System.Threading.Tasks;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Tasks;

namespace Incoding.Core.Block.Scheduler
{
    public class SchedulerTask : SequentialTask<GetSchedulersQuery.Response>
    {
        private SchedulerTaskOptions _options = new SchedulerTaskOptions();
        public class SchedulerTaskOptions
        {
            public bool Async { get; set; }
        }

        public SchedulerTask(Action<SchedulerTaskOptions> options)
        {
            options(_options);
            Command = (item) => new RunSchedulerCommand() { Item = item, Options = _options };
            Query = () => new GetSchedulersQuery()
            {
                Async = _options.Async,
                Date = (_options.Async ? GetSchedulersQuery.LastDateAsync : GetSchedulersQuery.LastDate).GetValueOrDefault(DateTime.UtcNow),
                FetchSize = 5
            };
            AfterExecution = async () =>
            {
                var dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
                var date = await dispatcher.QueryAsync(new GetSchedulersQuery.GetLastDateQuery()
                {
                    Async = _options.Async
                });
                if (_options.Async)
                    GetSchedulersQuery.LastDateAsync = date;
                else
                    GetSchedulersQuery.LastDate = date;
            };
        }

    }

    public static class BackgroundTaskFactoryExtensions
    {
        public static void AddScheduler(this BackgroundTaskFactory factory)
        {
            var syncTask = new SchedulerTask(options => options.Async = false);
            factory.AddSequentialExecutor("Scheduler",
                syncTask, options => options.AfterExecution = syncTask.AfterExecution);
            var asyncTask = new SchedulerTask(options => options.Async = true);
            factory.AddSequentialExecutor("SchedulerAsync", 
                asyncTask, options => options.AfterExecution = asyncTask.AfterExecution);
        }
    }
}