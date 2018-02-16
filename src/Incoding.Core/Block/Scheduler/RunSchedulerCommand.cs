using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Block.Scheduler.Command;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS;
using Incoding.Core.Extensions;
using Incoding.Core.Tasks;

namespace Incoding.Core.Block.Scheduler
{
    public class RunSchedulerCommand : SequentialTaskCommandBase<GetSchedulersQuery.Response>
    {
        public bool IsAsyncProcessing { get; set; }

        protected override void Execute()
        {
            Dispatcher.New().Push(new ChangeSchedulerStatusCommand { Id = Item.Id, Status = DelayOfStatus.InProgress });

            if (Item.TimeOut == 0)
                Item.TimeOut = 5 * 1000;

            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Dispatcher.New().Push(Item.Instance);
                    sw.Stop();

                    Dispatcher.New().Push(new ChangeSchedulerStatusCommand
                    {
                        Id = Item.Id,
                        Status = DelayOfStatus.Success,
                        Description = "Executed in {0} sec of {1} timeout".F(sw.Elapsed.TotalSeconds, Item.TimeOut)
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.New().Push(new ChangeSchedulerStatusCommand
                    {
                        Id = Item.Id,
                        Status = DelayOfStatus.Error,
                        Description = ex.ToString()
                    });
                    throw;
                }
            }, TaskCreationOptions.LongRunning);

            if (!IsAsyncProcessing)
                task.Wait(Item.TimeOut);
        }
    }
}