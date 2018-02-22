using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Core.ViewModel;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncDropDownControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Constructors

        public IncDropDownControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
            Data = new SelectList(new string[0]);
            Template = IncodingHtmlHelper.DropDownTemplateId;
            InitBind = JqueryBind.InitIncoding;            
        }

        #endregion
        
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

        #endregion

        #region Properties

        public JqueryBind InitBind { get; set; }

        public JqueryAjaxOptions Options { get { return this.options; } set { this.options = value; } }

        public IncSelectList Data { get; set; }

        public Selector Template { get; set; }

        #endregion

        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            string currentUrl = Data;
            bool isAjax = !string.IsNullOrWhiteSpace(currentUrl);

            var meta = isAjax ? this.htmlHelper.When(InitBind).Ajax(currentUrl)
                               : this.htmlHelper.When(InitBind);
            attributes = meta.OnSuccess(dsl =>
            {
                if (isAjax)
                {
                    dsl.Self().JQuery.Dom.Empty();
                    foreach (var vm in (List<KeyValueVm>)Data.Optional)
                    {
                        var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                        option.InnerHtml.Append(vm.Text);
                        option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), vm.Value);
                        dsl.Self().JQuery.Dom.Use(option.ToHtmlString()).Prepend();
                    }
                    dsl.Self().Insert.WithTemplate(Template).Append();
                }

                var selected = ExpressionMetadataProvider.FromLambdaExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider).Model;
                if (selected != null)
                    dsl.Self().JQuery.Attr.Val(selected);

                OnInit.Do(action => action(dsl));
                OnEvent.Do(action => action(dsl));
            })
                             .When(JqueryBind.Change)
                             .OnSuccess(dsl =>
                             {
                                 OnChange.Do(action => action(dsl));
                                 OnEvent.Do(action => action(dsl));
                             })
                             .AsHtmlAttributes(this.attributes);

            var tag = this.htmlHelper.DropDownListFor(this.property, isAjax ? new SelectList(new string[] { }) : (SelectList)Data, string.Empty, this.attributes);
            tag.WriteTo(writer, encoder);
        }
    }
}