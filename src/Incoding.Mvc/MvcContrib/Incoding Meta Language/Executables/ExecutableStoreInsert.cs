using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
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