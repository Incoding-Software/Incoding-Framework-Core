using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
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