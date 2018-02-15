using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableValidationRefresh : ExecutableBase
    {
        public ExecutableValidationRefresh(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
                this.Set("prefix", prefix);
        }
    }
}