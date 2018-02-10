using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Action
{
    public class ExecutableSubmitAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableSubmitAction(string formSelector, JqueryAjaxFormOptions ajaxForm)
        {            
            this.Add("formSelector", formSelector);
            this.Add("options", ajaxForm);
        }

        #endregion
    }
}