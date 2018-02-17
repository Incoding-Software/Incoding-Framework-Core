namespace Incoding.Web.MvcContrib
{
    public interface IIncodingMetaLanguageUtilitiesDsl
    {
        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
        
    }
}