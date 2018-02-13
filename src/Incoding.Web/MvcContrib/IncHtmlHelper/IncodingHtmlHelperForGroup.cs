﻿using System;
using System.Linq.Expressions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Incoding_Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib.IncHtmlHelper
{
    #region << Using >>

    #endregion

    public class IncodingHtmlHelperForGroup<TModel, TProperty> where TModel : new()
    {
        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncodingHtmlHelperForGroup(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        ////ncrunch: no coverage start
        #region Api Methods

        public IHtmlContent Static(Action<IncHorizontalControl<IncStaticControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncStaticControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent Hidden(Action<IncHiddenControl<TModel, TProperty>> configuration = null)
        {
            var hidden = new IncHiddenControl<TModel, TProperty>(htmlHelper, property);
            MaybeObject.Do(configuration, r => r(hidden));
            return hidden.ToHtmlString();
        }

        public IHtmlContent CheckBox(Action<IncHorizontalControl<IncCheckBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncCheckBoxControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent TextBox(Action<IncHorizontalControl<IncTextBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncTextBoxControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent File(Action<IncHorizontalControl<IncFileControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncFileControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent TextArea(Action<IncHorizontalControl<IncTextAreaControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncTextAreaControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent Password(Action<IncHorizontalControl<IncPasswordControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncPasswordControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent DropDown(Action<IncHorizontalControl<IncDropDownControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncDropDownControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent ListBox(Action<IncHorizontalControl<IncListBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncListBoxControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        public IHtmlContent RadioButton(Action<IncHorizontalControl<IncRadioButtonControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncRadioButtonControl<TModel, TProperty>(htmlHelper, property), configuration);
        }

        #endregion

        IHtmlContent Group<TInput>(TInput input, Action<IncHorizontalControl<TInput>> configuration) where TInput : IncControlBase
        {
            var label = new IncLabelControl(htmlHelper, property);
            label.AddClass("control-label");
            var validation = new IncValidationControl(htmlHelper, property);
            var horizontal = new IncHorizontalControl<TInput>(label, input, validation);
            MaybeObject.Do(configuration, r => r(horizontal));

            return horizontal.ToHtmlString();
        }

        ////ncrunch: no coverage end
    }
}