using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    public class IncodingMetaCallbackFuncDsl
    {
        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackFuncDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            _htmlHelper = htmlHelper;
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting IncrementVal(Selector step)
        {
            return this.plugInDsl.Core(_htmlHelper).JQuery.Call("increment", step);
        }

        public IExecutableSetting IncrementVal()
        {
            return IncrementVal(1);
        }

        public IExecutableSetting DecrementVal()
        {
            return IncrementVal(-1);
        }

        #endregion
    }
}