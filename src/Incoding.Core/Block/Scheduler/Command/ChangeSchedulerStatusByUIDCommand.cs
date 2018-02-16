using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Block.Scheduler.Command
{
    #region << Using >>

    #endregion

    public class ChangeSchedulerStatusByUIDCommand : CommandBase
    {
        protected override void Execute()
        {
            foreach (var delay in Repository.Query(whereSpecification: new DelayToScheduler.Where.ByUID(UID)))
            {
                Dispatcher.Push(new ChangeSchedulerStatusCommand
                                {
                                        Id = delay.Id,
                                        Status = Status
                                });
            }
        }

        #region Properties

        public string UID { get; set; }

        public DelayOfStatus Status { get; set; }

        #endregion
    }
}