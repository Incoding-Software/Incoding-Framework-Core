using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core.Block.Core;
using Incoding.Core.Block.IoC;
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

            public IMessage Instance { get; set; }

            public int TimeOut { get; set; }
        }

        protected override async Task<IEnumerable<Response>> ExecuteResult()
        {
            var delayOfStatuses = new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToList();
            if (FirstRun)
                delayOfStatuses.Add(DelayOfStatus.InProgress);
            
            var nowInFeature = Date.AddSeconds(2);
            var isHaveForDo = Async ? (!LastDateAsync.HasValue || LastDateAsync <= nowInFeature) : (!LastDate.HasValue || LastDate <= nowInFeature);
            if (!isHaveForDo)
                return new List<Response>();

            var items = Repository.Query(
                    whereSpecification: new DelayToScheduler.Where.ByStatus(delayOfStatuses.ToArray())
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
                .Select(s => new
                {
                    Id = s.Id,
                    Type = s.Type,
                    Command = s.Command,
                    Timeout = s.Timeout
                });

            var list = await items.ToProviderList();

            return list.Select(s =>
            {
                var command = s.Command.DeserializeFromJson(Type.GetType(s.Type)) as IMessage;
                return new Response()
                {
                    Id = s.Id,
                    Instance = command,
                    TimeOut = s.Timeout
                };
            }).ToArray();
        }
        
        public class GetLastDateQuery : QueryBaseAsync<DateTime?>
        {
            public bool Async { get; set; }
            
            protected override async Task<DateTime?> ExecuteResult()
            {
                var delayToSchedulers = Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToArray())
                    .And(new DelayToScheduler.Where.ByAsync(Async))
                    .And(new DelayToScheduler.Where.AvailableStartsOn(DateTime.UtcNow)));
                return await delayToSchedulers.ToProviderAny() ? await delayToSchedulers.ToProviderMin(s => s.StartsOn) : (DateTime?) null;
            }
        }
    }
}