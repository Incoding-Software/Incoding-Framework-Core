using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Incoding.WebTest.Operations
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
            await Repository.SaveAsync(new ItemEntity()
            {
                Name = OriginalValue1
            });
            Result = 5;
        }
    }
}