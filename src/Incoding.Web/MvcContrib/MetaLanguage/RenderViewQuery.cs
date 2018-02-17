using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
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