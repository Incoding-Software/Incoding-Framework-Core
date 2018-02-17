using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public abstract class ExecutableActionBase : ExecutableBase
    {
        #region Api Methods

        public void SetFilter(ConditionalBase filter)
        {
            this.Set("filterResult", filter.GetData());
        }

        #endregion
    }
}