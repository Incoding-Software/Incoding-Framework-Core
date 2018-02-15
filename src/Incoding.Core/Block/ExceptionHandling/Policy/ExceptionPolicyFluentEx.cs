using System;
using Incoding.Core.Block.Core;

namespace Incoding.Core.Block.ExceptionHandling.Policy
{
    #region << Using >>

    #endregion

    public static class ExceptionPolicyFluentEx
    {
        #region Factory constructors

        public static ExceptionPolicy Catch(this IsSatisfied<Exception> satisfied, Action<Exception> callback)
        {
            return new ExceptionPolicy(satisfied, exception =>
                                                      {
                                                          callback(exception);
                                                          return null;
                                                      });
        }

        public static ExceptionPolicy Mute(this IsSatisfied<Exception> satisfied)
        {
            return satisfied.Wrap(exception => null);
        }

        public static ExceptionPolicy ReThrow(this IsSatisfied<Exception> satisfied)
        {
            return satisfied.Wrap(exception => exception);
        }

        public static ExceptionPolicy Wrap(this IsSatisfied<Exception> satisfied, Func<Exception, Exception> evaluator)
        {
            return new ExceptionPolicy(satisfied, evaluator);
        }

        #endregion
    }
}