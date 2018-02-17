using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public static class ViewContextExtensions
    {
        #region Factory constructors

        public static bool IsAction(this ViewContext context, [AspMvcAction] string action)
        {
            string currentAction = TryGetRouteData(context, "action");
            return currentAction.Equals(action, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsArea(this ViewContext context, [AspMvcArea] string area)
        {
            string currentArea = TryGetRouteData(context, "area");
            return currentArea.Equals(area, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsController(this ViewContext context, [AspMvcController] string controller)
        {
            string currentController = TryGetRouteData(context, "controller");
            return currentController.Equals(controller, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsCurrent(this ViewContext context, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "")
        {
            bool res = IsController(context, controller) && IsAction(context, action);
            if (!string.IsNullOrWhiteSpace(area))
                res = IsArea(context, area);

            return res;
        }

        #endregion

        static string TryGetRouteData(this ViewContext context, string key)
        {
            return context.RouteData.DataTokens[key] != null
                           ? context.RouteData.DataTokens[key].ToString()
                           : string.Empty;
        }
    }
}