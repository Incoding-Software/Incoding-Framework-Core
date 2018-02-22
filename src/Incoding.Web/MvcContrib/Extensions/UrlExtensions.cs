using System;
using System.Web;
using Incoding.Core;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Web.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public static class UrlExtensions
    {
        #region Factory constructors

        public static string ActionArea(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area, object routes = null)
        {
            Guard.NotNullOrWhiteSpace("action", action);
            Guard.NotNullOrWhiteSpace("controller", controller);

            var routeValues = AnonymousHelper.ToDictionary(routes);
            routeValues.Set("area", area);
            return urlHelper.Action(action, controller, routeValues);
        }

        public static UrlDispatcher Dispatcher(this IUrlHelper urlHelper)
        {
            return new UrlDispatcher(urlHelper);
        }

        public static string Hash(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, object routes = null)
        {
            return InternalHash(urlHelper, action, controller, urlHelper.ActionContext.HttpContext.Request.GetUri(), string.Empty, routes);
        }

        public static string HashArea(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "", object routes = null)
        {
            return InternalHash(urlHelper, action, controller, urlHelper.ActionContext.HttpContext.Request.GetUri(), area, routes);
        }

        public static string HashReferral(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, object routes = null)
        {
            return InternalHash(urlHelper, action, controller, urlHelper.ActionContext.HttpContext.Request.GetUri(), string.Empty, routes);
        }

        public static string HashReferralArea(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "", object routes = null)
        {
            return InternalHash(urlHelper, action, controller, urlHelper.ActionContext.HttpContext.Request.GetUri(), area, routes);
        }

        #endregion

        static string InternalHash(this IUrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, Uri uri, [AspMvcArea] string area = "", object routes = null)
        {
            string baseUrl = "/";

            if (uri.If(r => r.Segments.Length > 1).Has())
            {
                baseUrl = baseUrl
                        .AppendSegment(uri.Segments[0])
                        .AppendSegment(uri.Segments[1].Replace("/", string.Empty));
            }

            string urlHash = StringUrlExtensions.AppendToHashQueryString(area
                    .Not(string.IsNullOrWhiteSpace)
                    .ReturnOrDefault(r => ActionArea(urlHelper, action, controller, area), urlHelper.Action(action, controller)), routes,true);

            return HttpUtility.UrlDecode(baseUrl.SetHash(urlHash));
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}