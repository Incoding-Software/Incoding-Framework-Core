#region copyright

// @incoding 2011
#endregion

using System;
using System.Collections.Generic;
using Incoding.Core.Block.ExceptionHandling.Policy;

namespace Incoding.Core.Block.ExceptionHandling
{
    #region << Using >>

    #endregion

    public sealed class InitExceptionHandling
    {
        #region Fields

        readonly List<ExceptionPolicy> exceptionPolicies = new List<ExceptionPolicy>();

        #endregion

        #region Api Methods

        public void WithPolicy(Func<ExceptionPolicy, ExceptionPolicy> func)
        {
            this.exceptionPolicies.Add(func(new ExceptionPolicy()));
        }

        #endregion

        internal List<ExceptionPolicy> GetPolicies()
        {
            return this.exceptionPolicies;
        }
    }
}