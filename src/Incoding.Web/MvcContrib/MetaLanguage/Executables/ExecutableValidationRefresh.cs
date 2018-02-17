using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
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