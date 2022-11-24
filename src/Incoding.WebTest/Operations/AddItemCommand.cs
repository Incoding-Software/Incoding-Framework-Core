using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentValidation;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Incoding.WebTest.Operations
{
    public class AddItemCommand : CommandBase
    {
        public string OriginalValue { get; set; }
        public List<Guid> ItemId { get; set; }
        public IFormFile F1 { get; set; }
        public List<int> TargetIds { get; set; }

        public class Validator : AbstractValidator<AddItemCommand>
        {
            public Validator()
            {
                RuleFor(r => r.OriginalValue).NotEmpty().Must(r =>
                {
                    int val;
                    return r != null && int.TryParse(r, out val) && val > 15;
                }).WithMessage("Value must be greater than 15");
            }
        }

        public class TestGenericCommand : CommandBase<DateTime>
        {
            protected override DateTime ExecuteResult()
            {
                //do some stuff, save to database
                return DateTime.UtcNow;
            }
        }

        protected override void Execute()
        {
            var dateTime = Dispatcher.Push(new TestGenericCommand());
            Debug.WriteLine(dateTime);
            Debug.WriteLine(F1);
            Repository.Save(new ItemEntity()
            {
                Name = OriginalValue
            });
            Result = 5;
        }

        public class AsView : QueryBase<AddItemCommand>
        {
            protected override AddItemCommand ExecuteResult()
            {
                return new AddItemCommand
                {
                    TargetIds = new List<int> {1555, 1777}
                };
            }
        }
    }
}