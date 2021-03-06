using System;
using System.Linq;
using Incoding.Core.Block.Core;

namespace Incoding.Core.Block.ExceptionHandling
{
    #region << Using >>

    #endregion

    public class ExceptionHandlingFactory : FactoryBase<InitExceptionHandling>
    {
        #region Static Fields

        static readonly Lazy<ExceptionHandlingFactory> instance = new Lazy<ExceptionHandlingFactory>(() => new ExceptionHandlingFactory());

        #endregion

        #region Properties

        public static ExceptionHandlingFactory Instance { get { return instance.Value; } }

        #endregion

        #region Api Methods

        public void Handler<TException>(TException exception) where TException : Exception
        {
            Guard.NotNull("exception", exception);

            var firstExceptionPolicy = this.init
                                           .GetPolicies()
                                           .FirstOrDefault(r => r.IsSatisfied(exception));

            if (firstExceptionPolicy == null)
                throw exception;

            var reThrowException = firstExceptionPolicy.Handle(exception);
            if (reThrowException != null)
                throw reThrowException;
        }

        #endregion
    }
}