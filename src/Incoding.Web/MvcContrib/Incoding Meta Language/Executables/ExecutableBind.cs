using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableBind : ExecutableBase
    {
        #region Constructors

        public ExecutableBind(string type, string meta, string bind)
        {
            this.Set("type", type);
            if (!string.IsNullOrWhiteSpace(meta))
                this.Set("meta", meta);
            if (!string.IsNullOrWhiteSpace(bind))
                this.Set("bind", bind);
        }

        #endregion
    }
}