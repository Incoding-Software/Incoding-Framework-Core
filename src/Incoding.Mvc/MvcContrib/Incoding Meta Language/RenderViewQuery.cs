namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language
{
    #region << Using >>

    #endregion

    public class RenderViewQuery : QueryBase<MvcHtmlString>
    {
        public HtmlHelper HtmlHelper { get; set; }

        public string PathToView { get; set; }

        public object Model { get; set; }

        protected override MvcHtmlString ExecuteResult()
        {
            return this.HtmlHelper.Partial(PathToView, Model);
        }
    }
}