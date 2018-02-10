using System.Linq;
using FluentValidation;
using Incoding.CQRS;

namespace Incoding.WebTest.Operations
{
    public class GetDoubleValueQuery : QueryBase<int>
    {
        public string OriginalValue { get; set; }

        public class Validator : AbstractValidator<GetDoubleValueQuery>
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

        protected override int ExecuteResult()
        {
            Dispatcher.New().Push(new SaveEntityCommand()
            {
                Value = OriginalValue
            });
            
            var names = Repository.Query<ItemEntity>().Select(r => r.Name).ToList();

            return int.Parse(OriginalValue) * 2;
        }
    }

    public class SaveEntityCommand : CommandBase
    {
        public string Value { get; set; }
        protected override void Execute()
        {
            Repository.Save(new ItemEntity()
            {
                Name = Value
            });
        }
    }
}