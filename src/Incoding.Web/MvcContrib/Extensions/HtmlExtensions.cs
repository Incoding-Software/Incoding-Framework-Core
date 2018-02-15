using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors;
using Incoding.Mvc.MvcContrib.MVD;
using Incoding.Mvc.MvcContrib.Template.Factory;
using Incoding.Web.MvcContrib;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Incoding.Web.MvcContrib.Services;
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
        #region Factory constructors

        public static HtmlDispatcher Dispatcher(this IHtmlHelper htmlHelper)
        {
            return new HtmlDispatcher(IoCFactory.Instance.TryResolve<IDispatcher>(), htmlHelper);
        }

        // ReSharper disable once UnusedParameter.Global
        public static IHtmlContent AsView<TData>(this HtmlDispatcher dispatcher, TData data, [AspMvcPartialView] string view, object model = null) where TData : class
        {
            return new HtmlString(IoCFactory.Instance.TryResolve<ITemplateFactory>().Render(dispatcher.HtmlHelper, view, data, model));
        }

        public static IHtmlContent AsViewFromQuery<TResult>(this HtmlDispatcher dispatcher, QueryBase<TResult> query, [AspMvcPartialView] string view, object model = null) where TResult : class
        {
            return dispatcher.AsView(dispatcher.Query(query), view, model);
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
            return new IncodingHtmlHelper<TModel>(htmlHelper);
        }
        
        public static SelectorHelper<TModel> Selector<TModel>(this IHtmlHelper<TModel> htmlHelper)
        {
            return new SelectorHelper<TModel>();
        }

        public static IIncodingMetaLanguageBindingDsl When(this IHtmlHelper htmlHelper, JqueryBind bind)
        {
            return htmlHelper.When(bind.ToJqueryString());
        }

        public static IIncodingMetaLanguageBindingDsl When(this IHtmlHelper htmlHelper, string bind)
        {
            return new IncodingMetaLanguageDsl(htmlHelper, bind);
        }

        #endregion
    }
}