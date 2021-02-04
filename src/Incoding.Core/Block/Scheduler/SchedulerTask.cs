using System;
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
            Command = (item) => new RunSchedulerCommand() { Item = item };
            Query = () => new GetSchedulersQuery()
            {
                Async = _options.Async,
                Date = (_options.Async ? GetSchedulersQuery.LastDateAsync : GetSchedulersQuery.LastDate).GetValueOrDefault(DateTime.UtcNow),
                FetchSize = 5
            };
            AfterExecution = () =>
            {
                var date = IoCFactory.Instance.TryResolve<IDispatcher>().Query(new GetSchedulersQuery.GetLastDateQuery()
                {
                    Async = _options.Async
                });
                if (_options.Async)
                    GetSchedulersQuery.LastDateAsync = date;
                else
                    GetSchedulersQuery.LastDate = date;
            };
        }

        public override Func<GetSchedulersQuery.Response, SequentialTaskCommandBase<GetSchedulersQuery.Response>> Command { get; set; }
        public override Func<SequentialTaskQueryBase<GetSchedulersQuery.Response>> Query { get; set; }
        public override Action AfterExecution { get; set; }
    }

    public static class BackgroundTaskFactoryExtensions
    {
        public static void AddScheduler(this BackgroundTaskFactory factory)
        {
            factory.AddSequentalExecutor("Scheduler",
                new SchedulerTask(options => options.Async = false));
            factory.AddSequentalExecutor("SchedulerAsync",
                new SchedulerTask(options => options.Async = true));
        }
    }
}