using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackWindowDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackWindowDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IncodingMetaCallbackConsoleDsl Console { get { return new IncodingMetaCallbackConsoleDsl(plugIn); } }

        public IExecutableSetting Alert(Selector message)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("alert", new[] { message }, "window"));
        }

        public IExecutableSetting Open(Selector url, string title)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("open", new object[] { url, title }, "window"));
        }

        public IExecutableSetting ClearInterval(string intervalId)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Window_Clear_Interval.F(intervalId)));
        }

        #endregion
    }
}