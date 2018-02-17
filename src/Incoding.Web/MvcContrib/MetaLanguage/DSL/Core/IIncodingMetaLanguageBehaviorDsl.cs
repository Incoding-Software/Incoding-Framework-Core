namespace Incoding.Web.MvcContrib
{
    public interface IIncodingMetaLanguageBehaviorDsl : IIncodingMetaLanguagePlugInDsl
    {
        void Lock();

        void UnLock();
    }
}