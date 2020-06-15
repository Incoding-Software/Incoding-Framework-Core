using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Core;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Incoding.WebTest.Operations
{
   
    public class GetItems1Query : QueryBaseAsync<List<GetItems1Query.Response>>, ICacheKey
    {
        
        public class Response
        {
            public string Key { get; set; }

            public string StringPresentation { get; set; }

        }

        protected override async Task<List<Response>> ExecuteResult()
        {
            var ent = await Repository.GetByIdAsync<ItemEntity>(Repository.Query<ItemEntity>().FirstOrDefault()?.Id ?? Guid.NewGuid());

            await Task.Delay(5000);
            
                var names = Repository.Query(whereSpecification: new ItemEntity.Where.ByStringLongerThan(3),
                    orderSpecification: new ItemEntity.Order.ByName()).Select(r => new Response
                {
                    Key = r.Id.ToString(),
                    StringPresentation = "The value is: " + r.Name
                }).ToList();
                return names;
        }

        public string GetName()
        {
            return nameof(GetItems1Query);
        }

        public class AsView : QueryBaseAsync<List<GetItems1Query.Response>>
        {
            protected override async Task<List<Response>> ExecuteResult()
            {
                var connection = IoCFactory.Instance.TryResolve<IConfiguration>().GetConnectionString("Main1");

                return await Dispatcher.QueryAsync(new GetItems1Query(), new MessageExecuteSetting {
                    Connection = connection
                    });
            }
        }
    }
}