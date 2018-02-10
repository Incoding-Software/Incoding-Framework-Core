namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncHelpBlockControl : IncControlBase
    {
        #region Properties

        public string Message { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return MvcHtmlString.Empty;

            var p = new TagBuilder(HtmlTag.P.ToStringLower());
            p.MergeAttributes(this.attributes, true);
            p.AddCssClass("help-block");
            p.SetInnerText(Message);
            return new MvcHtmlString(p.ToString());
        }
    }
}