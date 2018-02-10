using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackDocumentDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackDocumentDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting HistoryGo(int countGo)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("go", new object[] { countGo }, "history"));
        }

        public IExecutableSetting Back()
        {
            return HistoryGo(-1);
        }

        public IExecutableSetting Forward()
        {
            return HistoryGo(1);
        }


        public IExecutableSetting PushState(Selector rootOfRool, Selector url)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("PushState", new[] { rootOfRool,url }, "ExecutableHelper"));
            
        }

        public IExecutableSetting RedirectTo(Selector url)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("RedirectTo", new[] { url }, "ExecutableHelper"));
        }

        public IExecutableSetting RedirectToSelf()
        {
            return RedirectTo(Selector.JS.Location.Href);
        }

        public IExecutableSetting Reload(bool force = false)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Window_Location_Reload.F(force.ToString().ToLower())));
        }

        public IExecutableSetting Title(Selector title)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("setTitle", new[] { title }, "document"));
        }

        #endregion
    }
}