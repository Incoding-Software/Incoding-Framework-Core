using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
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