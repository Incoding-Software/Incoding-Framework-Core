using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Block.IoC;
using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors;
using Incoding.Mvc.MvcContrib.MVD;
using Incoding.Mvc.MvcContrib.Template.Factory;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Mvc.MvcContrib.Extensions
{
    #region << Using >>

    #endregion

    public static class HtmlExtensions
    {
        [ThreadStatic]
        internal static IHtmlHelper HtmlHelper;

        [ThreadStatic]
        internal static IUrlDispatcher UrlDispatcher;

        #region Factory constructors

        public static IDispatcher Dispatcher(this IHtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
            return IoCFactory.Instance.TryResolve<IDispatcher>();
        }

        // ReSharper disable once UnusedParameter.Global
        public static HtmlString AsView<TData>(this IDispatcher dispatcher, TData data, [PathReference] string view, object model = null) where TData : class
        {
            Guard.NotNull("HtmlHelper", HtmlHelper, "HtmlHelper != null");
            return new HtmlString(IoCFactory.Instance.TryResolve<ITemplateFactory>().Render(HtmlHelper, view, data, model));
        }

        public static HtmlString AsViewFromQuery<TResult>(this IDispatcher dispatcher, QueryBase<TResult> query, [PathReference] string view, object model = null) where TResult : class
        {
            return dispatcher.AsView(dispatcher.Query(query), view, model);
        }

        [Obsolete("Please use AsView or AsViewFromQuery with Html.Dispatcher().AsView(data) / Html.Dispatcher().AsViewFromQuery(query)", true), ExcludeFromCodeCoverage, UsedImplicitly]
        public static HtmlString AsView<TData>(this TData data, [PathReference] string view, object model = null) where TData : class
        {
            Guard.NotNull("HtmlHelper", HtmlHelper, "HtmlHelper != null");
            return new HtmlString(IoCFactory.Instance.TryResolve<ITemplateFactory>().Render(HtmlHelper, view, data, model));
        }

        public static IncodingHtmlHelperFor<TModel, TProperty> For<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) where TModel : new()
        {
            return new IncodingHtmlHelperFor<TModel, TProperty>(htmlHelper, property);
        }

        public static IncodingHtmlHelperForGroup<TModel, TProperty> ForGroup<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) where TModel : new()
        {
            return new IncodingHtmlHelperForGroup<TModel, TProperty>(htmlHelper, property);
        }

        public static IncodingHtmlHelper<TModel> Incoding<TModel>(this IHtmlHelper<TModel> htmlHelper) where TModel : new()
        {
            HtmlHelper = htmlHelper;
            return new IncodingHtmlHelper<TModel>(htmlHelper);
        }
        
        public static SelectorHelper<TModel> Selector<TModel>(this IHtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
            return new SelectorHelper<TModel>();
        }

        public static IIncodingMetaLanguageBindingDsl When(this IHtmlHelper htmlHelper, JqueryBind bind)
        {
            return htmlHelper.When(bind.ToJqueryString());
        }

        public static IIncodingMetaLanguageBindingDsl When(this IHtmlHelper htmlHelper, string bind)
        {
            HtmlHelper = htmlHelper;
            UrlDispatcher = new UrlDispatcher(new UrlHelper(htmlHelper.ViewContext));
            return new IncodingMetaLanguageDsl(htmlHelper, bind);
        }

        #endregion
    }
}