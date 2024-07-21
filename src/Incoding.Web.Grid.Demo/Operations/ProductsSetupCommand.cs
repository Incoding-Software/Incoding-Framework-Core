using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridUI.Operations;
using GridUI.Persistance;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;
using Incoding.Web.CQRS.Common.Query;

namespace GridUI.Setups
{
    public class ProductsSetupCommand : CommandBaseAsync
    {
        protected override async Task ExecuteAsync()
        {
            if (this.Dispatcher.Query(new GetEntitiesQuery<Product>()).Any())
                return;

            if (this.Dispatcher.Query(new GetEntitiesQuery<User>()).Any())
                return;
            
            var IgorCmd = new AddUserCommand
                              {
                                  FirstName = "Igor",
                                  LastName = "Valukhov"
                              };

            var VladCmd = new AddUserCommand
                              {
                                  FirstName = "Vlad",
                                  LastName = "Kopachinsky"
                              };

            var VictorCmd = new AddUserCommand
                              {
                                  FirstName = "Victor",
                                  LastName = "Gelmutdinov"
                              };

            await Dispatcher.PushAsync(IgorCmd);
            await Dispatcher.PushAsync(VictorCmd);
            await Dispatcher.PushAsync(VladCmd);

            for (int i = 0; i < 50; i++)
            {
                await Dispatcher.PushAsync(new AddProductCommand
                                     {
                                             Name = "Продукт " + i,
                                             Price = (decimal)55.5 + (decimal)i,
                                             Date = DateTime.Now.AddDays(-i),
                                             IsSoldOut = i%3 == 0,
                                             Users = i%2 == 0 ? 
                                             new List<User>() 
                                             {
                                                 (User)IgorCmd.Result, 
                                                 (User)VictorCmd.Result, 
                                                 (User)VladCmd.Result
                                             } : 
                                             new List<User>() 
                                             {
                                                 (User)IgorCmd.Result, 
                                             } 
                                     });
            }
        }
        
    }
}