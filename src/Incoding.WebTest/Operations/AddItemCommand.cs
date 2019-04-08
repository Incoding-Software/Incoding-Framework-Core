using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentValidation;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest.Operations
{
    public class AddItemCommand : CommandBase
    {
        public string OriginalValue { get; set; }
        public List<Guid> ItemId { get; set; }

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
            Repository.Save(new ItemEntity()
            {
                Name = OriginalValue
            });
        }
    }
}