using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Incoding.WebTest30.Operations
{
    public class GetDDEntitiesQuery : QueryBase<OptGroupVm>
    {
        public Guid? SelectedValue { get; set; }
        public List<Guid> SelectedValues { get; set; }

        protected override OptGroupVm ExecuteResult()
        {
            var entities = new List<ItemEntity>
            {
                new ItemEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString()
                },
                new ItemEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString()
                }
            };
            return new OptGroupVm(entities.Select(r => new KeyValueVm(r.Id, r.Name, r.Id == SelectedValue || (SelectedValues != null && SelectedValues.Contains(r.Id)))).ToList());
        }
    }
}