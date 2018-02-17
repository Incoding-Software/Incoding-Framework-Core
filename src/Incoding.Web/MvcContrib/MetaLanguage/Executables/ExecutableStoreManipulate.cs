using System.Collections.Generic;
using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
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