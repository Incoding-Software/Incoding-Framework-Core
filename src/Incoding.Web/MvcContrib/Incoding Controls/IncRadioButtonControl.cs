using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core;
using Incoding.Core.Extensions;
using Incoding.Core.Maybe;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Primitive;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncRadioButtonControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Constructors

        public IncRadioButtonControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
            this.Label = new IncLabelControl<TModel>(htmlHelper, property);
        }

        #endregion
        
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Properties

        public object Value { get; set; }

        public IncLabelControl<TModel> Label { get; protected set; }

        public string IconClass { get; set; }

        public ModeOfRadio Mode { get; set; }

        #endregion

        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            string value = Value.With(r => r.ToString());
            Guard.NotNullOrWhiteSpace("value", value, errorMessage: "Please set Value like are setting.Value = something");

            var div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.AddCssClass(Mode == ModeOfRadio.Normal ? B.Radio.ToLocalization() : B.Radio_inline.ToLocalization());
            var parentClass = GetAttributes().GetOrDefault(HtmlAttribute.Class.ToStringLower(), string.Empty).ToString();
            if (!string.IsNullOrEmpty(parentClass))
                div.AddCssClass(parentClass);
            var spanAsLabel = new TagBuilder(HtmlTag.Span.ToStringLower());
            spanAsLabel.InnerHtml.SetContent(this.Label.Name ?? value);
            var label = new TagBuilder(HtmlTag.Label.ToStringLower());
            label.TagRenderMode = TagRenderMode.Normal;
            var icon = new TagBuilder(HtmlTag.I.ToStringLower());
            if (!string.IsNullOrWhiteSpace(IconClass))
                icon.AddCssClass(IconClass);

            label.InnerHtml.AppendHtml(this.htmlHelper.RadioButtonFor(this.property, value, GetAttributes()).ToString()
                                       + icon
                                       + spanAsLabel);
            div.InnerHtml.AppendHtml(label);
            div.WriteTo(writer, encoder);
        }
    }
}