namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    #region << Using >>

    #endregion

    public static class IncodingMetaLanguageDslExtensions
    {
        #region Factory constructors

        public static void Behaviors(this IIncodingMetaLanguageCallbackInstancesDsl plugInDsl, Action<IIncodingMetaLanguageCallbackInstancesDsl> action)
        {
            var behaviorDsl = (IIncodingMetaLanguageBehaviorDsl)plugInDsl;
            behaviorDsl.Lock();
            action(plugInDsl);
            behaviorDsl.UnLock();
        }

        [Obsolete("Please skip because it is redundant")]
        public static IncodingMetaLanguageCoreDsl Core(this IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            return new IncodingMetaLanguageCoreDsl(plugInDsl);
        }

        #endregion
    }
}