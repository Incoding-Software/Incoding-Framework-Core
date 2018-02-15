using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Incoding.Extensions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Primitive;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncHorizontalControl<TInput, TModel> : IncControlBase<TModel> where TInput : IncControlBase<TModel>
    {
        #region Constructors

        public IncHorizontalControl(IncLabelControl<TModel> label, TInput input, IncControlBase<TModel> validation)
        {
            Label = label;
            Input = input;
            Validation = validation;
            HelpBlock = new IncHelpBlockControl<TModel>();
            Control = new IncDivControl<TModel>();
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
                Label.AddClass(IncodingHtmlHelper.Def_Label_Class.ToLocalization());

            if (!isV3orMore)
                Control.AddClass("controls");

            if (string.IsNullOrWhiteSpace(Control.GetAttr(HtmlAttribute.Class)))
            {
                Control.AddClass(isV3orMore
                    ? IncodingHtmlHelper.Def_Control_Class.ToLocalization()
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