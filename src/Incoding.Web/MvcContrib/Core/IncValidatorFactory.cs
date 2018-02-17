using System;
using FluentValidation;
using Incoding.Core.Block.IoC;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncValidatorFactory : IValidatorFactory
    {
        ////ncrunch: no coverage start

        #region Constructors

        public IncValidatorFactory()
        {
            //FluentValidationModelValidatorProvider.Configure();
        }

        #endregion

        ////ncrunch: no coverage end

        #region IValidatorFactory Members

        public IValidator<T> GetValidator<T>()
        {
            return IoCFactory.Instance.TryResolve<IValidator<T>>();
        }

        public IValidator GetValidator(Type type)
        {
            IValidator validator;
            try
            {
                var genericType = typeof(IValidator<>).MakeGenericType(new[] {type});
                validator = IoCFactory.Instance.TryResolve<IValidator>(genericType);
            }
            catch (InvalidOperationException)
            {
                validator = new ValidateNothingDecorator();
            }
            return validator;
        }

        internal sealed class ValidateNothingDecorator : AbstractValidator<object>
        {
        }

        #endregion
    }
}