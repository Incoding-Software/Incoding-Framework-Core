using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Incoding.Mvc.MvcContrib.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetUri(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}");
        }

        public static NameValueCollection GetNameValueCollection(this HttpRequest request)
        {
            NameValueCollection collection = new NameValueCollection();
            if (request.HasFormContentType)
            {
                foreach (KeyValuePair<string, StringValues> form in request.Form)
                {
                    collection.Add(form.Key, form.Value);
                }
            }
            foreach (KeyValuePair<string, StringValues> query in request.Query)
            {
                collection.Add(query.Key, query.Value);
            }
            return collection;
        }

        public static NameValueCollection GetNameValueCollection(this ActionContext context)
        {
            NameValueCollection collection = new NameValueCollection();
            if (context.HttpContext.Request.HasFormContentType)
            {
                foreach (KeyValuePair<string, StringValues> form in context.HttpContext.Request.Form)
                {
                    collection.Add(form.Key, form.Value);
                }
            }
            foreach (KeyValuePair<string, StringValues> query in context.HttpContext.Request.Query)
            {
                collection.Add(query.Key, query.Value);
            }
            foreach (var routeValue in context.RouteData.Values)
            {
                collection.Add(routeValue.Key, routeValue.Value as string);
            }
            return collection;
        }
    }
}