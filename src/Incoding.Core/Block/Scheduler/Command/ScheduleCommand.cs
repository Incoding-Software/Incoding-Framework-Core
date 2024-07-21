using System;
using System.Threading.Tasks;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Incoding.Core.Block.Scheduler.Command
{
    #region << Using >>

    #endregion

    public class ScheduleCommand : CommandBaseAsync
    {
        protected override async Task ExecuteAsync()
        {
            Recurrency = Recurrency ?? new GetRecurrencyDateQuery
                                       {
                                               Type = GetRecurrencyDateQuery.RepeatType.Once,
                                       };
            var type = Command.GetType();
            var option = type.FirstOrDefaultAttribute<OptionOfDelayAttribute>() ?? new OptionOfDelayAttribute();
            var startsOn = Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow);
            await Repository.SaveAsync(new DelayToScheduler
                            {
                                    Command = Command.ToJsonString(),
                                    CreateDt = DateTime.UtcNow,
                                    Type = type.AssemblyQualifiedName,
                                    UID = UID,
                                    Priority = Priority,
                                    Status = DelayOfStatus.New,
                                    Recurrence = Recurrency,
                                    StartsOn = startsOn,
                                    Option = new DelayToScheduler.OptionOfDelay()
                                             {
                                                     Async = option.Async,
                                                     TimeOut = option.TimeOutOfMillisecond
                                             }
                            });
            if (option.Async)
                GetSchedulersQuery.LastDateAsync = startsOn < GetSchedulersQuery.LastDateAsync ? startsOn : GetSchedulersQuery.LastDateAsync;
            else
                GetSchedulersQuery.LastDate = startsOn < GetSchedulersQuery.LastDate ? startsOn : GetSchedulersQuery.LastDate;

        }

        #region Constructors

        public ScheduleCommand(DelayToScheduler delay)
        {
            var instance = (CommandBase)delay.Command.DeserializeFromJson(Type.GetType(delay.Type));
            Command = instance;
            Priority = delay.Priority;
            UID = delay.UID;
            Setting = instance.Setting;
        }

        public ScheduleCommand() { }

        #endregion

        #region Properties

        public CommandBase Command { get; set; }

        public string UID { get; set; }

        public GetRecurrencyDateQuery Recurrency { get; set; }

        public int Priority { get; set; }

        #endregion
    }
}