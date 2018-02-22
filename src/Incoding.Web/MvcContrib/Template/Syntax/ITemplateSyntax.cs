using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public interface ITemplateSyntax<TModel> : IDisposable
    {
        ITemplateSyntax<TModel> Up();

        HtmlString For(string field);
        HtmlString For(IHtmlContent field);

        IncHtmlString For(Expression<Func<TModel, object>> field);

        IncHtmlString For(Expression<Func<TModel, bool>> field);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, IHtmlContent isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, IHtmlContent isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, string isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, IHtmlContent isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse);

        HtmlString IsInline(Expression<Func<TModel, object>> field, string content);

        HtmlString IsInline(Expression<Func<TModel, object>> field, IHtmlContent content);

        HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, string content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, IHtmlContent content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        HtmlString ForRaw(string field);

        HtmlString ForRaw(Expression<Func<TModel, object>> field);

        ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field);

        IDisposable Is(Expression<Func<TModel, object>> field);

        IDisposable Not(Expression<Func<TModel, object>> field);
    }
}