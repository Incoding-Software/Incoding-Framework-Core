using System.ComponentModel;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public enum HtmlType
    {
        [Description("text/html")]
        TextHtml, 

        [Description("text/xml")]
        TextXml, 

        [Description("text/javascript")]
        TextJavaScript, 

        [Description("text/css")]
        TextCss, 

        [Description("text/template")]
        TextTemplate, 
    }
}