using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Mvc.MvcContrib.Template.Syntax
{
    #region << Using >>

    #endregion

    public interface ITemplateSyntax<TModel> : IDisposable
    {
        ITemplateSyntax<TModel> Up();

        string For(string field);

        string For(Expression<Func<TModel, object>> field);

        string For(Expression<Func<TModel, bool>> field);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, HtmlString isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, HtmlString isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, string isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, HtmlString isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse);

        HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse);

        HtmlString IsInline(Expression<Func<TModel, object>> field, string content);

        HtmlString IsInline(Expression<Func<TModel, object>> field, HtmlString content);

        HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, string content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, HtmlString content);

        HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        HtmlString ForRaw(string field);

        HtmlString ForRaw(Expression<Func<TModel, object>> field);

        ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field);

        IDisposable Is(Expression<Func<TModel, object>> field);

        IDisposable Not(Expression<Func<TModel, object>> field);
    }
}