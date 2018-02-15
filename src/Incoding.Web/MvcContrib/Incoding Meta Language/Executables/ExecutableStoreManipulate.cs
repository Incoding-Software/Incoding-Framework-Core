using System.Collections.Generic;
using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableStoreManipulate : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreManipulate(string type, List<object> methods)
        {
            this.Add("type", type);
            this.Add("methods", ObjectExtensions.ToJsonString(methods));
        }

        #endregion
    }
}