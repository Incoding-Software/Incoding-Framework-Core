using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
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

        //[Obsolete("Please skip because it is redundant")]
        internal static IncodingMetaLanguageCoreDsl Core(this IIncodingMetaLanguagePlugInDsl plugInDsl, IHtmlHelper htmlHelper)
        {
            return new IncodingMetaLanguageCoreDsl(htmlHelper, plugInDsl);
        }

        #endregion
    }
}