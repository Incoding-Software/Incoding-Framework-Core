using System;
using System.Linq.Expressions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Incoding_Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib.IncHtmlHelper
{
    #region << Using >>

    #endregion

    public class IncodingHtmlHelperFor<TModel, TProperty> where TModel : new()
    {
        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncodingHtmlHelperFor(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        #region Api Methods

        public IHtmlContent Password(Action<IncPasswordControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncPasswordControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent TextArea(Action<IncTextAreaControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncTextAreaControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent TextBox(Action<IncTextBoxControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncTextBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent File(Action<IncFileControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncFileControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent DropDown(Action<IncDropDownControl<TModel, TProperty>> configuration)
        {
            return Control(new IncDropDownControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent ListBox(Action<IncListBoxControl<TModel, TProperty>> configuration)
        {
            return Control(new IncListBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent CheckBox(Action<IncCheckBoxControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncCheckBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent RadioButton(Action<IncRadioButtonControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncRadioButtonControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public IHtmlContent Hidden(Action<IncHiddenControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncHiddenControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        #endregion

        IHtmlContent Control<TInput>(TInput input, Action<TInput> configuration) where TInput : IncControlBase
        {
            configuration.Do(action => action(input));
            return input.ToHtmlString();
        }
    }
}