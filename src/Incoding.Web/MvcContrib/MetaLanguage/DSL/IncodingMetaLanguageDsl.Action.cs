using System;
using Incoding.Core;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Web.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public partial class IncodingMetaLanguageDsl 
    {
        #region IIncodingMetaLanguageActionDsl Members

        IIncodingMetaLanguageEventBuilderDsl AddAction(ExecutableActionBase action)
        {
            isEmptyAction = false;
            meta.Add(action);
            return this;
        }

        [Obsolete("Please use Selector.Event.Result")]
        public IIncodingMetaLanguageEventBuilderDsl Event()
        {
           return AddAction(new ExecutableEventAction());
        }

        [Obsolete(@"Method not more require")]
        public IIncodingMetaLanguageEventBuilderDsl Direct()
        {
            return AddAction(new ExecutableDirectAction(string.Empty));
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(IncodingResult result)
        {
            return AddAction(new ExecutableDirectAction(result.Data.ToJsonString()));
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(object result)
        {
            return Direct(IncodingResult.Success(result));
        }

        public IIncodingMetaLanguageEventBuilderDsl Submit(Action<JqueryAjaxFormOptions> configuration = null)
        {
            return SubmitOn(selector => selector.Self(), configuration);
        }

        //[Obsolete(@"Use Submit with option.Selector = selector ")]
        internal IIncodingMetaLanguageEventBuilderDsl SubmitOn(Func<JquerySelector, JquerySelector> action, Action<JqueryAjaxFormOptions> configuration = null)
        {
            var options = new JqueryAjaxFormOptions(JqueryAjaxFormOptions.Default);
            MaybeObject.Do(configuration, r => r(options));
            return AddAction(new ExecutableSubmitAction(action(Selector.Jquery), options));
        }

        public IIncodingMetaLanguageEventBuilderDsl Hash(string url = "", string prefix = "root")
        {
            return Hash((Action<JqueryAjaxOptions>) (r =>
            {
                r.Url = url;
                r.Type = JqueryAjaxOptions.HttpVerbs.Get;
            }), prefix);
        }

        [Obsolete("Use Hash")]
        public IIncodingMetaLanguageEventBuilderDsl AjaxHashGet(string url = "", string prefix = "root")
        {
            return Hash(url, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHashPost(string url = "", string prefix = "root")
        {
            return HashPost(url, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl HashPost(string url = "", string prefix = "root")
        {
            return AjaxHash(options =>
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                    options.Url = url;
                                options.Type = JqueryAjaxOptions.HttpVerbs.Post;
                            }, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl Hash(Action<JqueryAjaxOptions> configuration, string prefix = "root")
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            return AddAction(new ExecutableAjaxAction(true, prefix, options));
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxGet(string url)
        {
            return Ajax(url);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = JqueryAjaxOptions.HttpVerbs.Post;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost(IHtmlContent url)
        {
            return AjaxPost((string) url.HtmlContentToString());
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] Action<JqueryAjaxOptions> configuration)
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            Guard.NotNullOrWhiteSpace("url", options.Url);
            return AddAction(new ExecutableAjaxAction(false, string.Empty, options));
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = JqueryAjaxOptions.HttpVerbs.Get;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>([NotNull] TMessage message) where TMessage : IMessage, new()
        {
            return Ajax<TMessage>(message as object);
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>(object message = null) where TMessage : IMessage, new()
        {
            var baseType = typeof(TMessage).BaseType;
            while (baseType != typeof(object))
            {
                if (baseType == typeof(CommandBase))
                    return AjaxPost(r => r.Push<TMessage>(message));
                if (baseType.Name == "QueryBase`1")
                    return Ajax(r => r.Query<TMessage>(message).AsJson());

                baseType = baseType.BaseType;
            }
            throw new ArgumentException("Use Command or Query", "message");
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax(IHtmlContent url)
        {
            return Ajax(url.HtmlContentToString());
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHash(Action<JqueryAjaxOptions> configuration, string prefix = "root")
        {
            return Hash(configuration, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] Func<IUrlDispatcher, string> url)
        {
            var urlDispatcher = new UrlDispatcher(new UrlHelper(_htmlHelper.ViewContext));
            return Ajax(url(urlDispatcher));
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost([NotNull] Func<IUrlDispatcher, string> url)
        {
            var urlDispatcher = new UrlDispatcher(new UrlHelper(_htmlHelper.ViewContext));
            return AjaxPost(url(urlDispatcher));
        }

        #endregion
    }
}