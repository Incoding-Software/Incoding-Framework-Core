using System;
using System.Linq.Expressions;
using Incoding.Maybe;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    public class IncStaticControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncStaticControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            var tagBuilder = new TagBuilder("p");

            tagBuilder.InnerHtml.AppendHtml(ExpressionMetadataProvider
                    .FromLambdaExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider)
                    .Model.With(r => r.ToString()));

            tagBuilder.MergeAttributes(attributes, true);
            return tagBuilder;
        }
    }
}