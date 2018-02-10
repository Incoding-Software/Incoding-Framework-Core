using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Mvc.MvcContrib.Template.Syntax
{
    #region << Using >>

    #endregion

    [Obsolete("Please migrate to handlebars or another template engine", false)]
    public class TemplateMustacheSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Constructors

        public TemplateMustacheSyntax(Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper, string property, bool positiveConditional)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            string typeConditional = positiveConditional ? "#" : "^";
            htmlHelper.ViewContext.Writer.Write("{{" + typeConditional + property + "}}");
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("{{/" + this.property + "}}");
        }

        #endregion

        #region Fields

        readonly Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region ITemplateSyntax<TModel> Members

        [ExcludeFromCodeCoverage]
        public ITemplateSyntax<TModel> Up()
        {
            throw new NotImplementedException();
        }

        public string For(string field)
        {
            return "{{" + field + "}}";
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return "{{" + ReflectionExtensions.GetMemberName(field) + "}}";
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return StringExtensions.ToHtmlString((IsInline(field, isTrue).ToHtmlString() + NotInline(field, isFalse).ToHtmlString()));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, HtmlString isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), (string) isFalse.ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, HtmlString isFalse)
        {
            return Inline(field, isTrue, (string) isFalse.ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, string isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), isFalse);
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), (string) isFalse.Invoke(null).ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, HtmlString isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), (string) isFalse.ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), (string) isFalse.Invoke(null).ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue, (string) isFalse.Invoke(null).ToHtmlString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), isFalse);
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            string memberName = ReflectionExtensions.GetMemberName(field);
            return ("{{#" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}").ToHtmlString();
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            string memberName = ReflectionExtensions.GetMemberName(field);
            return ("{{^" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}").ToHtmlString();
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, new HtmlString(content.Invoke(null).ToHtmlString()));
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, new HtmlString(content.Invoke(null).ToHtmlString()));
        }

        public HtmlString ForRaw(string field)
        {
            return ("{{{" + field + "}}}").ToHtmlString();
        }

        public HtmlString ForRaw(Expression<Func<TModel, object>> field)
        {
            return ForRaw(ReflectionExtensions.GetMemberName(field));
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            return new TemplateMustacheSyntax<TNewModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), true);
        }

        public IDisposable Is(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), true);
        }

        public IDisposable Not(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), false);
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return IsInline(field, HtmlString.Create(content));
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return NotInline(field, HtmlString.Create(content));
        }

        #endregion
    }
}