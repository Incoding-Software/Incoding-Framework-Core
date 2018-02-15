using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncTextBoxControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncTextBoxControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.Placeholder" />
        /// </summary>
        public string Placeholder { set { this.attributes.Set(HtmlAttribute.Placeholder.ToStringLower(), value); } }

        public int MaxLength { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        public bool Autocomplete
        {
            set
            {
                if (value)
                    SetAttr(HtmlAttribute.AutoComplete);
                else
                    RemoveAttr(HtmlAttribute.AutoComplete);
            }
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.TextBoxFor(this.property, GetAttributes()).WriteTo(writer, encoder);
        }
    }
}