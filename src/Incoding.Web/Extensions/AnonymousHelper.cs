using System.Collections.Generic;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.Extensions
{
    #region << Using >>

    #endregion

    public static class AnonymousHelper
    {
        #region Factory constructors

        
        public static RouteValueDictionary ToDictionary(object anonymous)
        {
            if (anonymous == null)
                return new RouteValueDictionary();

            if (anonymous.GetType().IsAnyEquals(typeof(Dictionary<string, object>), typeof(IDictionary<string, object>)))
                return new RouteValueDictionary(anonymous as Dictionary<string, object> ?? new Dictionary<string, object>());

            return anonymous as RouteValueDictionary ?? new RouteValueDictionary(anonymous);
        }

        #endregion
    }
}