using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class TemplateDoTSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        string property;

        #endregion

        #region Constructors

        public TemplateDoTSyntax(IHtmlHelper htmlHelper, string property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;

            htmlHelper.ViewContext.Writer.Write("{{~it:x}}");
        }

        #endregion

        #region ITemplateSyntax<TModel> Members

        public ITemplateSyntax<TModel> Up()
        {
            throw new NotImplementedException();
        }

        public string For(string field)
        {
            return "{{=x." + field + "}}";
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, HtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, HtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, HtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            throw new NotImplementedException();
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            throw new NotImplementedException();
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            throw new NotImplementedException();
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            throw new NotImplementedException();
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            throw new NotImplementedException();
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            throw new NotImplementedException();
        }

        public HtmlString ForRaw(string field)
        {
            throw new NotImplementedException();
        }

        public HtmlString ForRaw(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            throw new NotImplementedException();
        }

        public IDisposable Is(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        public IDisposable Not(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            htmlHelper.ViewContext.Writer.Write("{{~}}");
        }

        #endregion

        #region Enums

        public enum DoTType
        { }

        #endregion
    }
}