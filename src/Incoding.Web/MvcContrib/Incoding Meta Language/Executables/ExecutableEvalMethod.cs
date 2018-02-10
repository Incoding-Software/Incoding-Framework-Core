using System.Linq;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public class ExecutableEvalMethod : ExecutableBase
    {
        #region Constructors

        public ExecutableEvalMethod(string method, object[] args, string context)
        {
            this.Set("method", method);
            this.Set("args", args.Select((r) =>
                                             {
                                                 if (r is Selector)
                                                     return (r as Selector).ToString();
                                                 return r.ToString();
                                             })
                                 .ToArray());
            this.Set("context", context);
        }

        #endregion
    }
}