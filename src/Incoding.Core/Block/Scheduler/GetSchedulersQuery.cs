using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.Core;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.Core.Tasks;

namespace Incoding.Core.Block.Scheduler
{
    public class GetSchedulersQuery : SequentialTaskQueryBase<GetSchedulersQuery.Response>
    {
        private static bool FirstRun = true;

        public static DateTime? LastDateAsync { get; set; }
        public static DateTime? LastDate { get; set; }
        
        public int FetchSize { get; set; }

        public bool Async { get; set; }

        public DateTime Date { get; set; }
        
        public class Response
        {
            public string Id { get; set; }

            public CommandBase Instance { get; set; }

            public int TimeOut { get; set; }
        }

        protected override IEnumerable<Response> ExecuteResult()
        {
            var delayOfStatuses = new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToList();
            if (FirstRun)
                delayOfStatuses.Add(DelayOfStatus.InProgress);
            
            var nowInFeature = Date.AddSeconds(2);
            var isHaveForDo = Async ? (!LastDateAsync.HasValue || LastDateAsync <= nowInFeature) : (!LastDate.HasValue || LastDate <= nowInFeature);
            if (!isHaveForDo)
                return new List<Response>();

            return Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(delayOfStatuses.ToArray())
                        .And(new DelayToScheduler.Where.ByAsync(Async))
                        .And(new DelayToScheduler.Where.AvailableStartsOn(Date)),
                    orderSpecification: new DelayToScheduler.Sort.Default(),
                    paginatedSpecification: new PaginatedSpecification(1, FetchSize))
                .Select(s => new
                {
                    Id = s.Id,
                    Command = s.Command,
                    Timeout = s.Option.TimeOut,
                    Type = s.Type
                })
                .Select(s => new Response()
                {
                    Id = s.Id,
                    Instance = s.Command.DeserializeFromJson(Type.GetType(s.Type)) as CommandBase,
                    TimeOut = s.Timeout
                })
                .ToList();
        }
        
        public class GetLastDateQuery : QueryBase<DateTime?>
        {
            public bool Async { get; set; }
            
            protected override DateTime? ExecuteResult()
            {
                var delayToSchedulers = Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToArray())
                    .And(new DelayToScheduler.Where.ByAsync(Async))
                    .And(new DelayToScheduler.Where.AvailableStartsOn(DateTime.UtcNow)));
                return delayToSchedulers.Any() ? delayToSchedulers.Min(s => s.StartsOn) : (DateTime?) null;
            }
        }
    }
}