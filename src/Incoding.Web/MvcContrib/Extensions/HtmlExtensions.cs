using System;
using System.Linq.Expressions;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
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