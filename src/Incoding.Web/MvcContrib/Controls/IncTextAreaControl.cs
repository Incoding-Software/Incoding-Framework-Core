using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncTextAreaControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncTextAreaControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.Cols" />
        /// </summary>
        public int Cols
        {
            set { this.attributes.Set(HtmlAttribute.Cols.ToStringLower(), value.ToString()); }
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.Rows" />
        /// </summary>
        public int Rows
        {
            set { this.attributes.Set(HtmlAttribute.Rows.ToStringLower(), value.ToString()); }
        }

        public int MaxLenght { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.TextAreaFor(this.property, 5, 25, GetAttributes()).WriteTo(writer, encoder);
        }
    }
}