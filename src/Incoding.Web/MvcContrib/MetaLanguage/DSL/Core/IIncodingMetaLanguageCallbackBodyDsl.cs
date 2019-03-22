using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
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