using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Core;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Incoding.WebTest80.Operations
{
    public class ClearItemCacheCommand : CommandBase
    {
        protected override void Execute()
        {
            //CachingFactory.Instance.Delete(new GetItemsQuery());
            //CachingFactory.Instance.Delete(new GetItems1Query());
            CachingFactory.Instance.DeleteAll();
        }
    }

    public class GetWithParams : QueryBase<OptGroupVm>
    {
        protected override OptGroupVm ExecuteResult()
        {
            return new OptGroupVm(new List<KeyValueVm> { new KeyValueVm(1555, "Item1")});
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
            public int X1 { get; set; }
            public string X2 { get; set; }
            public string X3 { get; set; }
            public string X4 { get; set; }
            protected override List<Response> ExecuteResult()
            {
                var connection = IoCFactory.Instance.TryResolve<IConfiguration>().GetConnectionString("Main");

                return Dispatcher.Query(new GetItemsQuery(), new MessageExecuteSetting {
                    Connection = connection
                    });
            }
        }
    }
}