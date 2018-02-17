using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Incoding.Core;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class TemplateHandlebarsSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        readonly string property;

        readonly HandlebarsType type;

        string level;

        #endregion

        #region Constructors

        public TemplateHandlebarsSyntax(IHtmlHelper htmlHelper, string property, HandlebarsType type, string level)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.type = type;
            this.level = level;

            htmlHelper.ViewContext.Writer.Write("{{#" + type.ToStringLower() + " " + this.level + this.property + "}}");
        }

        #endregion

        #region ITemplateSyntax<TModel> Members

        public ITemplateSyntax<TModel> Up()
        {
            level += "../";
            return this;
        }

        public string For(string field)
        {
            return Build("{{" + level + field + "}}");
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return Build("{{#if " + level + ReflectionExtensions.GetMemberName(field) + "}}true{{else}}false{{/if}}");
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return Build("{{#if " + level + ReflectionExtensions.GetMemberName(field) + "}}" + isTrue + "{{else}}" + isFalse + "{{/if}}").ToMvcHtmlString();
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, HtmlString isFalse)
        {
            return Inline(field, isTrue.ToString(), (string) isFalse.ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, HtmlString isFalse)
        {
            return Inline(field, isTrue, isFalse.ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, string isFalse)
        {
            return Inline(field, isTrue.ToString(), isFalse);
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue.Invoke(null).ToString(), isFalse.Invoke(null).ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, HtmlString isFalse)
        {
            return Inline(field, isTrue.Invoke(null).ToString(), isFalse.ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, HtmlString isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue.ToString(), isFalse.Invoke(null).ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue, isFalse.Invoke(null).ToString());
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            return Inline(field, isTrue.Invoke(null).ToString(), isFalse);
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            return IsInline(field, (string) content.ToString());
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, HtmlString content)
        {
            return NotInline(field, (string) content.ToString());
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, (string) content.Invoke(null).ToString());
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, (string) content.Invoke(null).ToString());
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return Build("{{#unless " + level + ReflectionExtensions.GetMemberName(field) + "}}" + content + "{{/unless}}").ToMvcHtmlString();
        }

        public HtmlString ForRaw(string field)
        {
            return Build("{{{" + level + field + "}}}").ToMvcHtmlString();
        }

        public HtmlString ForRaw(Expression<Func<TModel, object>> field)
        {
            return ForRaw(ReflectionExtensions.GetMemberName(field));
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            return BuildNew<TNewModel>(ReflectionExtensions.GetMemberName(field), HandlebarsType.Each);
        }

        public IDisposable Is(Expression<Func<TModel, object>> field)
        {
            return BuildNew<TModel>(field, HandlebarsType.If);
        }

        public IDisposable Not(Expression<Func<TModel, object>> field)
        {
            return BuildNew<TModel>(field, HandlebarsType.Unless);
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return Build("{{#if " + level + ReflectionExtensions.GetMemberName(field) + "}}" + content + "{{/if}}").ToMvcHtmlString();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            htmlHelper.ViewContext.Writer.Write("{{/" + type.ToStringLower() + "}}");
        }

        #endregion

        ITemplateSyntax<T> BuildNew<T>(string newProperty, HandlebarsType newType)
        {
            var res = new TemplateHandlebarsSyntax<T>(htmlHelper, newProperty, newType, level);
            level = string.Empty;
            return res;
        }

        ITemplateSyntax<T> BuildNew<T>(Expression<Func<TModel, object>> field, HandlebarsType newType)
        {
            if (field.Body.NodeType != ExpressionType.MemberAccess)
            {
                var expression = field.Body as UnaryExpression;
                Guard.IsConditional("field", expression.With(r => r.Operand.NodeType) == ExpressionType.MemberAccess, errorMessage: Resources.Exception_Handlerbars_Only_Member_Access);
            }

            return BuildNew<T>(ReflectionExtensions.GetMemberName(field), newType);
        }

        string Build(string res)
        {
            level = string.Empty;
            return res;
        }
    }

    #region Enums

    #endregion
}