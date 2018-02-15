using System;
using Incoding.Core.CQRS.Core;
using Incoding.CQRS;
using Incoding.Mvc.MvcContrib.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Options;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Jquery;
using Incoding.Mvc.MvcContrib.MVD;
using Microsoft.AspNetCore.Html;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageSettingEventDsl : IIncodingMetaLanguageActionDsl, IIncodingMetaLanguageEventBuilderDsl { }

    public interface IIncodingMetaLanguageActionDsl
    {
        IIncodingMetaLanguageEventBuilderDsl Ajax(Action<JqueryAjaxOptions> configuration);

        IIncodingMetaLanguageEventBuilderDsl Ajax(string url);

        IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>(TMessage message = default(TMessage)) where TMessage : IMessage, new();

        IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>(object message = null) where TMessage : IMessage, new();

        IIncodingMetaLanguageEventBuilderDsl Ajax(HtmlString url);

        IIncodingMetaLanguageEventBuilderDsl Ajax(Func<IUrlDispatcher, string> url);

        [Obsolete("Please use Selector.Event.Result")]
        IIncodingMetaLanguageEventBuilderDsl Event();

        [Obsolete(@"Method not more require")]
        IIncodingMetaLanguageEventBuilderDsl Direct();

        IIncodingMetaLanguageEventBuilderDsl Direct(IncodingResult result);

        IIncodingMetaLanguageEventBuilderDsl Direct(object data);

        IIncodingMetaLanguageEventBuilderDsl Submit(Action<JqueryAjaxFormOptions> configuration = null);

        //[Obsolete(@"Use Submit with option.Selector = selector ")]
        //IIncodingMetaLanguageEventBuilderDsl SubmitOn(Func<JquerySelector, JquerySelector> action, Action<JqueryAjaxFormOptions> configuration = null);

        IIncodingMetaLanguageEventBuilderDsl Hash(string url = "", string prefix = "root");

        [Obsolete("Use Hash")]
        IIncodingMetaLanguageEventBuilderDsl AjaxHashGet(string url = "", string prefix = "root");

        [Obsolete("Use HashPost")]
        IIncodingMetaLanguageEventBuilderDsl AjaxHashPost(string url = "", string prefix = "root");

        IIncodingMetaLanguageEventBuilderDsl HashPost(string url = "", string prefix = "root");

        [Obsolete("Use Hash")]
        IIncodingMetaLanguageEventBuilderDsl AjaxHash(Action<JqueryAjaxOptions> configuration, string prefix = "root");

        IIncodingMetaLanguageEventBuilderDsl Hash(Action<JqueryAjaxOptions> configuration, string prefix = "root");

        [Obsolete("Use Ajax")]
        IIncodingMetaLanguageEventBuilderDsl AjaxGet(string url);

        IIncodingMetaLanguageEventBuilderDsl AjaxPost(string url);

        IIncodingMetaLanguageEventBuilderDsl AjaxPost(HtmlString url);

        IIncodingMetaLanguageEventBuilderDsl AjaxPost(Func<IUrlDispatcher, string> url);

        IIncodingMetaLanguageSettingEventDsl PreventDefault();

        IIncodingMetaLanguageSettingEventDsl StopPropagation();
    }
}