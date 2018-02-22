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

        public HtmlString For(string field)
        {
            return Build("{{" + level + new HtmlString(field) + "}}");
        }

        public HtmlString For(IHtmlContent field)
        {
            return Build("{{" + level + field + "}}");
        }

        public IncHtmlString For(Expression<Func<TModel, object>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public IncHtmlString For(Expression<Func<TModel, bool>> field)
        {
            return Build("{{#if " + level + ReflectionExtensions.GetMemberName(field) + "}}true{{else}}false{{/if}}");
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return Inline(field, new HtmlString(isTrue), new HtmlString(isFalse));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, IHtmlContent isFalse)
        {
            return Build(new HtmlString("{{#if " + level + ReflectionExtensions.GetMemberName(field) + "}}").Concat(isTrue).Concat("{{else}}").Concat(isFalse).Concat("{{/if}}"));
        }
        
        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, IHtmlContent isFalse)
        {
            return Inline(field, new HtmlString(isTrue), isFalse);
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, string isFalse)
        {
            return Inline(field, isTrue, new HtmlString(isFalse));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue.Invoke(null), isFalse.Invoke(null));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, IHtmlContent isFalse)
        {
            return Inline(field, isTrue.Invoke(null), isFalse);
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, IHtmlContent isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue, isFalse.Invoke(null));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue, isFalse.Invoke(null));
        }

        public HtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            return Inline(field, isTrue.Invoke(null), isFalse);
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, content.Invoke(null));
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return IsInline(field, new HtmlString(content));
        }

        public HtmlString IsInline(Expression<Func<TModel, object>> field, IHtmlContent content)
        {
            return Build(new HtmlString("{{#if ").Concat(level, ReflectionExtensions.GetMemberName(field), "}}").Concat(content).Concat("{{/if}}"));
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, content.Invoke(null));
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return NotInline(field, new HtmlString(content));
        }

        public HtmlString NotInline(Expression<Func<TModel, object>> field, IHtmlContent content)
        {
            return Build(new HtmlString("{{#unless " + level + ReflectionExtensions.GetMemberName(field) + "}}").Concat(content).Concat("{{/unless}}"));
        }

        public HtmlString ForRaw(IHtmlContent field)
        {
            return Build(new HtmlString("{{{" + level).Concat(field).Concat("}}}"));
        }

        public HtmlString ForRaw(string field)
        {
            return ForRaw(new HtmlString(field));
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

        HtmlString Build(IHtmlContent res)
        {
            level = string.Empty;
            return res.ToHtmlString();
        }

        HtmlString Build(string res)
        {
            level = string.Empty;
            return new HtmlString(res);
        }
    }

    #region Enums

    #endregion
}