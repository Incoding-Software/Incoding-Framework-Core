using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;
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

        public void Push(CommandBase command)
        {
            _dispatcherImplementation.Push(command);
        }

        public T Push<T>(CommandBase<T> command)
        {
            _dispatcherImplementation.Push(command);
            return command.Result;
        }

        public async Task PushAsync(CommandBaseAsync command)
        {
            await _dispatcherImplementation.PushAsync(command);
        }

        public async Task<T> PushAsync<T>(CommandBaseAsync<T> command)
        {
            await _dispatcherImplementation.PushAsync(command);
            return command.Result;
        }
        
        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            return _dispatcherImplementation.Query(message, executeSetting);
        }

        public async Task<TResult> QueryAsync<TResult>(QueryBaseAsync<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            return await _dispatcherImplementation.QueryAsync(message, executeSetting);
        }
    }
}