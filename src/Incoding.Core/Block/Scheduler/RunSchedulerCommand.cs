using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Block.Scheduler.Command;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.Core.Tasks;

namespace Incoding.Core.Block.Scheduler
{
    public class RunSchedulerCommand : SequentialTaskCommandBase<GetSchedulersQuery.Response>
    {
        protected override async Task ExecuteAsync()
        {
            await Dispatcher.New().PushAsync(new ChangeSchedulerStatusCommand { Id = Item.Id, Status = DelayOfStatus.InProgress });

            if (Item.TimeOut == 0)
                Item.TimeOut = 5 * 1000;

            var task = Dispatcher.PushAsync(new ProcessMessage
            {
                Item = Item
            });

            if (Options.Async)
                task.ContinueWith(r => { Console.WriteLine($"RunScheduler: {r.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
            else 
                task.Wait(Item.TimeOut == 0 ? DefaultSyncTimeout : Item.TimeOut);
        }

        public static int DefaultSyncTimeout { get; set; } = 60000; // 1 minute
        public SchedulerTask.SchedulerTaskOptions Options { get; set; }
    }

    public class ProcessMessage : CommandBaseAsync
    {
        public GetSchedulersQuery.Response Item { get; set; }

        protected override async Task ExecuteAsync()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if(Item.Instance is CommandBaseAsync)
                    await Dispatcher.New().PushAsync(Item.Instance as CommandBaseAsync);
                else
                    Dispatcher.New().Push(Item.Instance as CommandBase);
                sw.Stop();

                await Dispatcher.New().PushAsync(new ChangeSchedulerStatusCommand
                {
                    Id = Item.Id,
                    Status = DelayOfStatus.Success,
                    Description = "Executed in {0} sec of {1} timeout".F(sw.Elapsed.TotalSeconds, Item.TimeOut)
                });
            }
            catch (Exception ex)
            {
                await Dispatcher.New().PushAsync(new ChangeSchedulerStatusCommand
                {
                    Id = Item.Id,
                    Status = DelayOfStatus.Error,
                    Description = ex.ToString()
                });
                throw;
            }
        }
    }
}