using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Incoding.Extensions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Options;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Primitive;
using Incoding.Mvc.MvcContrib.ViewModel;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncDropDownControl<TModel, TProperty> : IncControlBase
    {
        #region Constructors

        public IncDropDownControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            Data = new SelectList(new string[0]);
            Template = IncodingHtmlHelper.DropDownTemplateId;
            InitBind = JqueryBind.InitIncoding;            
        }

        #endregion

        public override IHtmlContent ToHtmlString()
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
                                                    dsl.Self().JQuery.Dom.Use(new HtmlString(option.ToString()).ToString()).Prepend();
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

            return this.htmlHelper.DropDownListFor(this.property, isAjax ? new SelectList(new string[] { }) : (SelectList)Data, string.Empty, this.attributes);
        }

        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

        #endregion

        #region Properties

        public JqueryBind InitBind { get; set; }

        public JqueryAjaxOptions Options { get { return this.options; } set { this.options = value; } }

        public IncSelectList Data { get; set; }

        public Selector Template { get; set; }

        #endregion
    }
}