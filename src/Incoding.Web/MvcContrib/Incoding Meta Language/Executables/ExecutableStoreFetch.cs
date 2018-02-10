using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
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