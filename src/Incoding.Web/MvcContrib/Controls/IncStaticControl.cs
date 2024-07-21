using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core;
using Incoding.Core.Block.IoC;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if netcoreapp2_1
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace Incoding.Web.MvcContrib
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

            tagBuilder.InnerHtml.AppendHtml(
#if netcoreapp2_1
                ExpressionMetadataProvider.FromLambdaExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider).Model
#else
                    IoCFactory.Instance.TryResolve<IModelExpressionProvider>().CreateModelExpression(htmlHelper.ViewData, property).Model
#endif
                .With(r => r.ToString()));

            tagBuilder.MergeAttributes(attributes, true);
            tagBuilder.WriteTo(writer, encoder);
        }
    }
}