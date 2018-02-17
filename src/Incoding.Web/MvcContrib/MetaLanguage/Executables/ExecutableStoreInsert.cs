using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ExecutableStoreInsert : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreInsert(string type, bool replace, string prefix)
        {
            this.Set("type", type);
            this.Set("replace", replace);
            this.Set("prefix", prefix);
        }

        #endregion
    }
}