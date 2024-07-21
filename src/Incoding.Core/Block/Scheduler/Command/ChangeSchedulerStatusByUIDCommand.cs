using System.Threading.Tasks;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Block.Scheduler.Command
{
    #region << Using >>

    #endregion

    public class ChangeSchedulerStatusByUIDCommand : CommandBaseAsync
    {
        protected override async Task ExecuteAsync()
        {
            foreach (var delay in Repository.Query(whereSpecification: new DelayToScheduler.Where.ByUID(UID)))
            {
                await Dispatcher.PushAsync(new ChangeSchedulerStatusCommand
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