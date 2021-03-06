using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class MvcScriptTemplate<TModel> : MvcTemplate<TModel>
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcScriptTemplate(IHtmlHelper htmlHelper, string id)
                : base(htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            this.htmlHelper.ViewContext.Writer.Write("<script id=\"{0}\" type=\"{1}\" >".F(id, HtmlType.TextTemplate.ToLocalization()));
        }

        #endregion

        public override void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("</script>");
        }
    }
}