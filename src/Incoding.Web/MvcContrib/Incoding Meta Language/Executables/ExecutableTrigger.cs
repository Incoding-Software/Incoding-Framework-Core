using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableTrigger : ExecutableBase
    {
        #region Constructors

        public ExecutableTrigger(string trigger, string property)
        {
            this.Set("trigger", trigger);
            if (!string.IsNullOrWhiteSpace(property))
                this.Set("property", property);
        }

        #endregion
    }
}