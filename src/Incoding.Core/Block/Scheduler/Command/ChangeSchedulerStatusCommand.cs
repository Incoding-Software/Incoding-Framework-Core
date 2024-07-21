using System.Threading.Tasks;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Block.Scheduler.Command
{
    #region << Using >>

    #endregion

    public class ChangeSchedulerStatusCommand : CommandBaseAsync
    {
        protected override async Task ExecuteAsync()
        {
            var delay = await Repository.GetByIdAsync<DelayToScheduler>(Id);
            delay.Status = Status;
            delay.Description = Description;

            if (Status == DelayOfStatus.Success)
            {
                if (delay.Recurrence != null)
                {
                    var recurrency = new GetRecurrencyDateQuery
                    {
                        EndDate = delay.Recurrence.EndDate,
                        RepeatCount = delay.Recurrence.RepeatCount - 1,
                        RepeatDays = delay.Recurrence.RepeatDays,
                        RepeatInterval = delay.Recurrence.RepeatInterval,
                        StartDate = delay.StartsOn,
                        Type = delay.Recurrence.Type
                    };
                    recurrency.NowDate =
                        delay.StartsOn; // calculate next start depending on previously calculated start (to run every day at exactly same time for example)
                    recurrency.StartDate = Dispatcher.Query(recurrency);
                    if (recurrency.StartDate.HasValue)
                    {
                        await Dispatcher.PushAsync(new ScheduleCommand(delay)
                        {
                            UID = delay.UID,
                            Priority = delay.Priority,
                            Recurrency = recurrency,
                        });
                        return;
                    }
                }
                if(DelayToScheduler.RemoveAfterSuccess)
                    Repository.Delete(delay);
            }
        }

        #region Properties

        public string Id { get; set; }

        public DelayOfStatus Status { get; set; }

        public string Description { get; set; }

        #endregion
    }
}