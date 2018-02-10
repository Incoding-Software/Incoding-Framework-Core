using Incoding.CQRS;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language
{
    #region << Using >>

    #endregion

    public class RenderViewQuery : QueryBase<IHtmlContent>
    {
        public IHtmlHelper HtmlHelper { get; set; }

        public string PathToView { get; set; }

        public object Model { get; set; }

        protected override IHtmlContent ExecuteResult()
        {
            return this.HtmlHelper.Partial(PathToView, Model);
        }
    }
}