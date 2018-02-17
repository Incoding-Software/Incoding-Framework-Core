using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ExecutableForm : ExecutableBase
    {
        #region Constructors

        public ExecutableForm(string method)
        {
            this.Set("method", method);
        }

        #endregion
    }
}