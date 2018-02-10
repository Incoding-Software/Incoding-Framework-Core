using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Mvc.MvcContrib.Primitive
{
    #region << Using >>

    #endregion

    public struct IncHtmlString
    {
        private readonly HtmlString result;

        public IncHtmlString(HtmlString result)
        {
            this.result = result;
        }

        public override string ToString()
        {
            return this.result.Value;
        }

        public static implicit operator string(IncHtmlString value)
        {
            return value.ToString();
        }

        public static implicit operator HtmlString(IncHtmlString value)
        {
            return value.result;
        }

        public static implicit operator IncHtmlString(string content)
        {
            return new IncHtmlString(new HtmlString(content));
        }

        public static implicit operator IncHtmlString(HtmlString content)
        {
            return new IncHtmlString(content);
        }

        public static implicit operator IncHtmlString(Func<object, HelperResult> content)
        {
            return new IncHtmlString(new HtmlString(content.Invoke(null).ToString()));
        }
    }
}