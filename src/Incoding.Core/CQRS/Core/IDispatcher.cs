#region copyright

// @incoding 2011

#endregion

using System;

namespace Incoding.CQRS
{
    /// <summary>
    ///     Interface Dispatcher
    /// </summary>
    public interface IDispatcher
    {
        IDispatcher New();

        void Push(CommandComposite composite);
        
        TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null);
    }
}