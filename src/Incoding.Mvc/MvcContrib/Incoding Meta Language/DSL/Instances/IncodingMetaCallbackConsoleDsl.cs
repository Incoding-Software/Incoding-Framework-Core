namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    public class IncodingMetaCallbackConsoleDsl
    {
        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        public IncodingMetaCallbackConsoleDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        public IExecutableSetting Log(string logType, Selector message)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("log", new object[] { logType, message }, "window.console"));
        }
    }
}