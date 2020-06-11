#region copyright

// @incoding 2011

#endregion

using System.Threading.Tasks;

namespace Incoding.Core.CQRS.Core
{
    /// <summary>
    ///     Interface Dispatcher
    /// </summary>
    public interface IDispatcher
    {
        IDispatcher New();

        void Push(CommandComposite composite);
        void Push(CommandBase command);
        T Push<T>(CommandBase<T> command);
        Task PushAsync(CommandBaseAsync command);
        Task<T> PushAsync<T>(CommandBaseAsync<T> command);

        TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null);
        Task<TResult> QueryAsync<TResult>(QueryBaseAsync<TResult> message, MessageExecuteSetting executeSetting = null);
    }
}