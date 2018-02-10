using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Action
{
    public class ExecutableAjaxAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableAjaxAction(bool hash, string prefix, JqueryAjaxOptions ajax)
        {            
            this["ajax"] = ajax;
            this["hash"] = hash;
            this["prefix"] = prefix;
        }

        #endregion
    }
}