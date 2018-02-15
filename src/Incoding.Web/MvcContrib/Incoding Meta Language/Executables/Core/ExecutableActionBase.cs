using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Condionals.Instances;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core
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