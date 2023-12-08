using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest80.Operations
{
    public class AddItemAsyncCommand : CommandBaseAsync
    {
        public string OriginalValue1 { get; set; }
        
        public class Validator : AbstractValidator<AddItemAsyncCommand>
        {
            public Validator()
            {
                RuleFor(r => r.OriginalValue1).NotEmpty().Must(r =>
                {
                    int val;
                    return r != null && int.TryParse(r, out val) && val > 15;
                }).WithMessage("Value must be greater than 15");
            }
        }

        protected override async Task ExecuteAsync()
        {
            await Task.Delay(10000);

            await Repository.SaveAsync(new ItemEntity()
            {
                Name = OriginalValue1
            });
            Result = 5;
        }
    }
}