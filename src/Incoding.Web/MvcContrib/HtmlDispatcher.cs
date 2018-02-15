using Incoding.Core.CQRS.Core;
using Incoding.CQRS;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    public class HtmlDispatcher : IDispatcher
    {
        public HtmlDispatcher(IDispatcher dispatcherImplementation, IHtmlHelper htmlHelper)
        {
            _dispatcherImplementation = dispatcherImplementation;
            HtmlHelper = htmlHelper;
        }

        private IDispatcher _dispatcherImplementation;
        public IHtmlHelper HtmlHelper { get; }
        public IDispatcher New()
        {
            return _dispatcherImplementation.New();
        }

        public void Push(CommandComposite composite)
        {
            _dispatcherImplementation.Push(composite);
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            return _dispatcherImplementation.Query(message, executeSetting);
        }
    }
}