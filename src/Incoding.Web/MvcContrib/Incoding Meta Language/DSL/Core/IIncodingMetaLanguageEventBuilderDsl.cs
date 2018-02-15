using System;
using System.Linq.Expressions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageEventBuilderDsl
    {
        IIncodingMetaLanguageEventBuilderDsl Where<TModel>([NotNull] Expression<Func<TModel, bool>> expression);

        IIncodingMetaLanguageEventBuilderDsl OnSuccess([NotNull] Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnError([NotNull] Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnComplete([NotNull] Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnBegin([NotNull] Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnBreak([NotNull] Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        HtmlRouteValueDictionary AsHtmlAttributes([CanBeNull] object htmlAttributes);

        HtmlRouteValueDictionary AsHtmlAttributes();

        HtmlRouteValueDictionary AsHtmlAttributes(string id = "", string classes = "", bool disabled = false, bool readOnly = false,
                                                     bool autocomplete = false, string placeholder = "", string title = "");

        [Obsolete("Please use using(ToBeginTag) instead of string", false)]
        string AsStringAttributes(object htmlAttributes = null);

        IIncodingMetaLanguageBindingDsl When([NotNull] JqueryBind nextBind);

        IIncodingMetaLanguageBindingDsl When([NotNull] string nextBind);
    }
}