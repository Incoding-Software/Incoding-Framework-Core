using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Core;
using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest.Operations
{
    public class ClearItemCacheCommand : CommandBase
    {
        protected override void Execute()
        {
            CachingFactory.Instance.Delete(new GetItemsQuery());
        }
    }

    public class GetItemsQuery : QueryBase<List<GetItemsQuery.Response>>, ICacheKey
    {
        public class Response
        {
            public string Key { get; set; }

            public string StringPresentation { get; set; }

        }

        protected override List<Response> ExecuteResult()
        {
            var result = CachingFactory.Instance.Retrieve(this, () =>
            {
                var names = Repository.Query(whereSpecification: new ItemEntity.Where.ByStringLongerThan(3),
                    orderSpecification: new ItemEntity.Order.ByName()).Select(r => new Response
                {
                    Key = r.Id.ToString(),
                    StringPresentation = "The value is: " + r.Name
                }).ToList();
                return names;
            });
            
            return result;
        }

        public string GetName()
        {
            return nameof(GetItemsQuery);
        }

        public class AsView : QueryBase<List<GetItemsQuery.Response>>
        {
            protected override List<Response> ExecuteResult()
            {
                return Dispatcher.Query(new GetItemsQuery());
            }
        }
    }
}