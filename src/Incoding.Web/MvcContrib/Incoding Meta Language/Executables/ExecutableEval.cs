using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableEval : ExecutableBase
    {
        #region Constructors

        public ExecutableEval(string code)
        {
            this.Set("code", code);
        }

        #endregion
    }
}