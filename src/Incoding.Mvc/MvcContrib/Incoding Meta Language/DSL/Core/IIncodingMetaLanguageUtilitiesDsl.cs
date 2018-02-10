using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    public interface IIncodingMetaLanguageUtilitiesDsl
    {
        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
        
    }
}