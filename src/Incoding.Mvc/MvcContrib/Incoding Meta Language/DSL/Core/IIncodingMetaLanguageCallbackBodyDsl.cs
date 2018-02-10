using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageCallbackBodyDsl : IIncodingMetaLanguageWithDsl
    {        
        IExecutableSetting Break { get; }

        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
    }
}