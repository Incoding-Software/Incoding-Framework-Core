using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Primitive;

namespace Incoding.Mvc.MvcContrib.Template
{
    #region << Using >>

    #endregion

    public class MvcScriptTemplate<TModel> : MvcTemplate<TModel>
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcScriptTemplate(HtmlHelper htmlHelper, string id)
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