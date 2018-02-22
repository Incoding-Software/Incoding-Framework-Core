using System;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public struct IncHtmlString
    {
        private readonly IHtmlContent result;

        public IncHtmlString(IHtmlContent result)
        {
            this.result = result;
        }

        public override string ToString()
        {
            return this.result.HtmlContentToString();
        }

        public static implicit operator string(IncHtmlString value)
        {
            return value.ToString();
        }

        public static implicit operator HtmlString(IncHtmlString value)
        {
            return new HtmlString(value.result.HtmlContentToString());
        }

        public static implicit operator IncHtmlString(string content)
        {
            return new IncHtmlString(new HtmlString(content));
        }

        public static implicit operator IncHtmlString(HtmlString content)
        {
            return new IncHtmlString(content);
        }

        public static implicit operator Selector(IncHtmlString content)
        {
            return new Selector(content.ToString());
        }

        
    }
}