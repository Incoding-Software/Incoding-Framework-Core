﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Core.Block.IoC;
using Incoding.Core.Quality;
using Incoding.Web.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class UrlDispatcher : IUrlDispatcher
    {
        internal static readonly ConcurrentDictionary<string, bool> duplicates = new ConcurrentDictionary<string, bool>();

        #region Static Fields

        public static bool IsVerifySchema;

        #endregion

        #region Fields

        readonly IUrlHelper urlHelper;
        readonly HttpContext httpContext;

        #endregion

        #region Constructors

        public UrlDispatcher(IUrlHelper urlHelper, HttpContext httpContext)
        {
            this.urlHelper = urlHelper;
            this.httpContext = httpContext;
        }

        #endregion

        static RouteValueDictionary GetQueryString(Dictionary<Type, List<object>> dictionary)
        {
            var query = new RouteValueDictionary();
            foreach (var pair in dictionary)
            {
                var valueAsRoutes = pair.Value
                                        .Where(s => s != null)
                                        .Select(o =>
                                                {
                                                    if (o is IDictionary<string, object> || o is IDictionary<string, string>)
                                                        return new RouteValueDictionary(o);
                                                    var res = new RouteValueDictionary();
                                                    var type = o.GetType();

                                                    foreach (var keys in (ReflectionExtensions.IsAnonymous(type) ? type.GetProperties() : type
                                                                                                                              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                                                                              .Where(r => !r.HasAttribute<IgnoreDataMemberAttribute>())
                                                                                                                              .Where(r => r.CanWrite)))
                                                        res.Add(keys.Name, o.TryGetValue(keys.Name));
                                                    return res;
                                                })
                                        .ToList();

                if (valueAsRoutes.Count > 1)
                {
                    for (int i = 0; i < valueAsRoutes.Count; i++)
                    {
                        foreach (var keyValue in valueAsRoutes[i].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                            query.Add("[{0}].{1}".F(i, keyValue.Key), keyValue.Value.ToString());
                    }
                }
                else if (valueAsRoutes.Count == 1)
                {
                    foreach (var keyValue in valueAsRoutes[0].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                        query.Add(keyValue.Key, keyValue.Value.ToString());
                }
            }

            return query;
        }

        void VerifySchema<TOriginal>(object routes)
        {
            if (!IsVerifySchema || routes == null)
                return;

            foreach (var property in routes.GetType().GetProperties())
            {
                var type = typeof(TOriginal);
                if (type.GetProperties().All(r => r.Name != property.Name))
                    throw new ArgumentOutOfRangeException("routes", "Can't found property {0} on {1}".F(property.Name, type.Name));
            }
        }

        static string GetTypeName(Type type)
        {
            string mainName = type.IsNested ? type.FullName : (duplicates.GetOrAdd(type.Name, (i) =>
                                                             {
                                                                 return AppDomain.CurrentDomain.GetAssemblies()
                                                                                 .Select(r => ReflectionExtensions.GetLoadableTypes(r))
                                                                                 .SelectMany(r => r)
                                                                                 .Count(s => s.Name == type.Name) > 1;
                                                             }) ? type.FullName : type.Name);
            return (type.IsGenericType ? "{0}{1}{2}".F(mainName, separatorByPair, StringExtensions.AsString(type.GetGenericArguments().Select(GetTypeName), separatorByGeneric)) : mainName)
                .Replace("+", "-"); // Url safety replacing
        }

        public interface IUrlQuery<TQuery>
        {
            string Validate();

            UrlQuery<TQuery> EnableValidate();

            string AsFile(string incContentType = "", string incFileDownloadName = "");

            string AsJson();

            string AsView([AspMvcPartialView, JetBrains.Annotations.NotNull] string incView);
        }

        #region Constants

        internal const string separatorByGeneric = "/";

        internal const string separatorByPair = "|";

        internal const string separatorByType = "&";

        #endregion

        #region IUrlDispatcher Members

        public IUrlQuery<TQuery> Query<TQuery>(object routes = null) where TQuery : new()
        {
            VerifySchema<TQuery>(routes);
            return new UrlQuery<TQuery>(urlHelper, httpContext, routes);
        }

        public IUrlQuery<TQuery> Query<TQuery>(TQuery routes) where TQuery : new()
        {
            return Query<TQuery>(routes: routes as object);
        }

        public UrlPush Push<TCommand>(TCommand routes) where TCommand : new()
        {
            return Push<TCommand>(routes: routes as object);
        }

        public UrlPush Push<TCommand>(object routes = null) where TCommand : new()
        {
            VerifySchema<TCommand>(routes);
            var res = new UrlPush(urlHelper, httpContext);
            return res.Push<TCommand>(routes);
        }

        public string AsView([AspMvcPartialView] string incView)
        {
            // ReSharper disable once Mvc.ActionNotResolved
            // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
            
            string url = urlHelper.Action("Render", "Dispatcher", new
                                                            {
                                                                    incView = incView,
                                                            });
#else
            string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext, "Render", "Dispatcher", new RouteValueDictionary(
            new {
                incView = incView,
            }));
#endif
            
            return url;
        }

        public UrlModel<TModel> Model<TModel>(object routes = null)
        {
            VerifySchema<TModel>(routes);
            var type = routes == null ? typeof(TModel) : routes.GetType();
            return new UrlModel<TModel>(urlHelper, httpContext, type.IsTypicalType() ? new { incValue = routes } : routes);
        }

        public UrlModel<TModel> Model<TModel>(TModel routes)
        {
            return Model<TModel>(routes as object);
        }

        #endregion

        #region Nested classes

        public class UrlModel<TModel>
        {
            #region Constructors

            public UrlModel(IUrlHelper urlHelper, HttpContext httpContext, object model)
            {
                defaultRoutes = new RouteValueDictionary
                                {
                                        { "incType", GetTypeName(typeof(TModel)) },
                                        { "incIsModel", true },
                                };

                this.urlHelper = urlHelper;
                this.httpContext = httpContext;
                this.model = model;
            }

            #endregion

            #region Api Methods

            public string AsView([AspMvcPartialView, JetBrains.Annotations.NotNull] string incView)
            {
                defaultRoutes.Add("incView", incView);
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                
                string url = urlHelper.Action("Render", "Dispatcher", defaultRoutes);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext, "Render", "Dispatcher", defaultRoutes);
#endif

                
                return StringUrlExtensions.AppendToQueryString(url, GetQueryString(new Dictionary<Type, List<object>>() { { typeof(TModel), new List<object>() { this.model } } }));
            }

            #endregion

            #region Fields

            readonly IUrlHelper urlHelper;
            readonly HttpContext httpContext;

            readonly object model;

            readonly RouteValueDictionary defaultRoutes;

            #endregion
        }

        public class UrlQuery<TQuery> : IUrlQuery<TQuery>
        {
            #region Constructors

            public UrlQuery(IUrlHelper urlHelper, HttpContext httpContext, object query)
            {
                defaultRoutes = new RouteValueDictionary();
                defaultRoutes.Add("incType", GetTypeName(typeof(TQuery)));
                this.urlHelper = urlHelper;
                this.httpContext = httpContext;
                this.query = GetQueryString(new Dictionary<Type, List<object>>()
                                            {
                                                    { typeof(TQuery), new List<object>() { query } }
                                            });
                //this.query.Add("incType", GetTypeName(typeof(TQuery)));
            }

            #endregion

            public override string ToString()
            {
                return AsJson();
            }

            #region Fields

            readonly IUrlHelper urlHelper;
            readonly HttpContext httpContext;

            readonly RouteValueDictionary query;

            readonly RouteValueDictionary defaultRoutes;

            #endregion

            #region IUrlQuery<TQuery> Members

            public string Validate()
            {
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                string url = urlHelper.Action("Validate", "Dispatcher", defaultRoutes);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>()
                    .GetUriByAction(httpContext, "Validate", "Dispatcher", defaultRoutes);
#endif
                return url;
            }

            public UrlQuery<TQuery> EnableValidate()
            {
                defaultRoutes.Add("incValidate", true);
                return this;
            }

            public string AsFile(string incContentType = "", string incFileDownloadName = "")
            {
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                string url = urlHelper.Action("QueryToFile", "Dispatcher", defaultRoutes);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext,
                    "QueryToFile", "Dispatcher", defaultRoutes);
#endif
                
                return StringUrlExtensions.AppendToQueryString(url, new
                    {
                        incContentType = incContentType,
                        incFileDownloadName = incFileDownloadName
                    })
                                .AppendToQueryString(query);
            }

            public string AsJson()
            {
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                string url = StringUrlExtensions.AppendToQueryString(urlHelper.Action("Query", "Dispatcher", defaultRoutes), query);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext, "Query", "Dispatcher", defaultRoutes)
                    .AppendToQueryString(query);
#endif
                return url;
            }

            public string AsView(string incView)
            {
                defaultRoutes.Add("incView", incView);
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                string url = urlHelper.Action("Render", "Dispatcher", defaultRoutes);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext, "Render", "Dispatcher", defaultRoutes);
#endif

                return StringUrlExtensions.AppendToQueryString(url, query);
            }

            #endregion
        }

        public class UrlPush
        {
            public override string ToString()
            {
                var routeValues = new RouteValueDictionary { { "incTypes", StringExtensions.AsString(dictionary.Select(r => GetTypeName(r.Key)), separatorByType) } };
                if (isCompositeAsArray)
                    routeValues.Add("incIsCompositeAsArray", true);
                if (onlyValidate)
                    routeValues.Add("incOnlyValidate", true);

                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
#if netcoreapp2_1
                string url = urlHelper.Action("Push", "Dispatcher", routeValues);
#else
                string url = IoCFactory.Instance.TryResolve<LinkGenerator>().GetUriByAction(httpContext, "Push", "Dispatcher", routeValues);
#endif

                return StringUrlExtensions.AppendToQueryString(url, GetQueryString(this.dictionary));
            }

            public static implicit operator string(UrlPush s)
            {
                return s.ToString();
            }

            #region Fields

            readonly IUrlHelper urlHelper;
            readonly HttpContext httpContext;

            readonly Dictionary<Type, List<object>> dictionary = new Dictionary<Type, List<object>>();

            bool onlyValidate;

            bool isCompositeAsArray;

            #endregion

            #region Constructors

            [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
            public UrlPush() { }

            public UrlPush(IUrlHelper urlHelper, HttpContext httpContext)
            {
                this.urlHelper = urlHelper;
                this.httpContext = httpContext;
            }

            #endregion

            #region IUrlPush Members

            public UrlPush OnlyValidate()
            {
                onlyValidate = true;
                return this;
            }

            public UrlPush Push<TCommand>(object routes)
            {
                var type = typeof(TCommand);
                bool isContains = dictionary.ContainsKey(type);
                if (isContains)
                    isCompositeAsArray = true;

                if (isContains)
                    dictionary[type].Add(routes);
                else
                    dictionary.Add(type, new List<object> { routes });

                return this;
            }

            public UrlPush Push<TCommand>(TCommand command)
            {
                return Push<TCommand>(command as object);
            }

            #endregion
        }

        #endregion
    }
}