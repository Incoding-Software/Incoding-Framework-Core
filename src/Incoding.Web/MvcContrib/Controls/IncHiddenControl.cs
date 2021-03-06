﻿using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncHiddenControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncHiddenControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.HiddenFor(this.property, GetAttributes()).WriteTo(writer, encoder);
        }
    }
}