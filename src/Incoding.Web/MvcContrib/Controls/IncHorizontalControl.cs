using System;
using System.IO;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Incoding.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncHorizontalControl<TInput, TModel> : IncControlBase<TModel> where TInput : IncControlBase<TModel>
    {
        #region Constructors

        public IncHorizontalControl(IHtmlHelper<TModel> htmlHelper, IncLabelControl<TModel> label, TInput input, IncControlBase<TModel> validation) : base(htmlHelper)
        {
            Label = label;
            Input = input;
            Validation = validation;
            HelpBlock = new IncHelpBlockControl<TModel>(htmlHelper);
            Control = new IncDivControl<TModel>(htmlHelper);
        }

        #endregion

        #region Properties

        public IncLabelControl<TModel> Label { get; set; }

        public TInput Input { get; set; }

        public IncDivControl<TModel> Control { get; set; }

        public IncControlBase<TModel> Validation { get; set; }

        public IncHelpBlockControl<TModel> HelpBlock { get; set; }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Func<IncControlBase<TModel>, bool> isForDefClass = @base => !@base.GetAttr(HtmlAttribute.Class).With(r => r.Contains("col-"));
            bool isV3orMore = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3;
            bool isStatic = Input.GetType().Name.Contains("IncStaticControl");

            AddClass(isV3orMore ? B.Form_group.ToLocalization() : "control-group");

            Label.AddClass(B.Control_label);
            if (isV3orMore && isForDefClass(Label))
                Label.AddClass(IncodingHtmlHelper.Def_Label_CustomClass ?? IncodingHtmlHelper.Def_Label_Class.ToLocalization());

            if (!isV3orMore)
                Control.AddClass("controls");

            if (string.IsNullOrWhiteSpace(Control.GetAttr(HtmlAttribute.Class)))
            {
                Control.AddClass(isV3orMore
                    ? (IncodingHtmlHelper.Def_Control_CustomClass ?? IncodingHtmlHelper.Def_Control_Class.ToLocalization())
                    : isStatic ? string.Empty : "control-group");
            }

            
            if (isV3orMore && !typeof(TInput).Name.Contains("IncCheckBoxControl"))
                Input.AddClass(isStatic ? B.Form_static_control.ToLocalization() : B.Form_control.ToLocalization());
            Control.Content = Input;
            
            TagBuilder div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.MergeAttributes(GetAttributes(), true);
            div.InnerHtml.AppendHtml(Label);
            div.InnerHtml.AppendHtml(Control);
            div.InnerHtml.AppendHtml(Validation);
            div.InnerHtml.AppendHtml(HelpBlock);

            div.WriteTo(writer, encoder);
        }
    }
}