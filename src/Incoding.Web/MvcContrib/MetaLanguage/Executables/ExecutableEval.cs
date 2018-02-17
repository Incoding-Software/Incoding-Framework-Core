using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ExecutableEval : ExecutableBase
    {
        #region Constructors

        public ExecutableEval(string code)
        {
            this.Set("code", code);
        }

        #endregion
    }
}