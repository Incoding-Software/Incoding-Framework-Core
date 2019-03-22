using System;
using System.Collections.Generic;
using System.Linq;
using GridUI.Persistance;
using GridUI.Specifications;
using Incoding.Core.CQRS.Core;
using Incoding.Web.Grid.Demo.Models;

namespace GridUI.Queries
{
    public class UserByProduct
    {
        public class AsView : QueryBase<List<UserVm>>
        {
            public string ProductId { get; set; }

            protected override List<UserVm> ExecuteResult()
            {
                return new List<UserVm>()
                {
                    new UserVm(new User()
                    {
                        FirstName = "One",
                        LastName = "User"
                    }),
                    new UserVm(new User()
                    {
                        FirstName = "Two",
                        LastName = "User"
                    })
                };
                //return Repository.Query(whereSpecification: new UsersByProductWhereSpec(Guid.Parse(ProductId))).ToList()
                //        .Select(r => new UserVm(r)).ToList();
            }
        }
    }
}