namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    public interface IIncodingMetaLanguageBehaviorDsl : IIncodingMetaLanguagePlugInDsl
    {
        void Lock();

        void UnLock();
    }
}