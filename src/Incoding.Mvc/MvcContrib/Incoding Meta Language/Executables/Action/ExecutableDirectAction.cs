using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Action
{
    public class ExecutableDirectAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableDirectAction(string result)
        {
            this["result"] = result;
        }

        #endregion
    }
}