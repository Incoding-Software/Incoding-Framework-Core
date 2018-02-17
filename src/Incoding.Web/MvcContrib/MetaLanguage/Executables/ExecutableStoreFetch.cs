using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ExecutableStoreFetch : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreFetch(string type, string prefix)
        {
            this.Set("type", type);
            this.Set("prefix", prefix);
        }

        #endregion
    }
}