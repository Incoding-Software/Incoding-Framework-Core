using System;
using JetBrains.Annotations;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    #region << Using >>

    #endregion


    public interface IIncodingMetaLanguageBindingDsl : IIncodingMetaLanguageActionDsl, IIncodingMetaLanguageEventBuilderDsl
    {
        [Obsolete("Not use because is default")]
        IIncodingMetaLanguageActionDsl Do();

        [UsedImplicitly, Obsolete("Use PreventDefault()")]
        IIncodingMetaLanguageActionDsl DoWithPreventDefault();

        [UsedImplicitly, Obsolete("Use StopPropagation()")]
        IIncodingMetaLanguageActionDsl DoWithStopPropagation();

        [UsedImplicitly, Obsolete("Use combine PreventDefault and StopPropagation()")]
        IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation();
    }
}