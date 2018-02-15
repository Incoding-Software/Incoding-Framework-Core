using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Maybe;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    public class IncStaticControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncStaticControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var tagBuilder = new TagBuilder("p");

            tagBuilder.InnerHtml.AppendHtml(ExpressionMetadataProvider
                .FromLambdaExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider)
                .Model.With(r => r.ToString()));

            tagBuilder.MergeAttributes(attributes, true);
            tagBuilder.WriteTo(writer, encoder);
        }
    }
}