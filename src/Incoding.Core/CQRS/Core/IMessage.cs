using System;
using System.Threading.Tasks;
using Incoding.Core.Data;

namespace Incoding.Core.CQRS.Core
{
    #region << Using >>

    #endregion

    /// <summary>
    /// Base interface for CQRS Messages
    /// </summary>
    public interface IMessage 
    {
        /// <summary>
        /// Message Result
        /// </summary>
        object Result { get; }
        
        /// <summary>
        /// Message Settings
        /// </summary>
        MessageExecuteSetting Setting { get; set; }

        /// <summary>
        /// On Message Execute
        /// </summary>
        /// <param name="current">Current Dispatcher</param>
        /// <param name="unitOfWork">Unit of Work</param>
        void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork);
        /// <summary>
        /// On Message Execute Async
        /// </summary>
        /// <param name="current">Current Dispatcher</param>
        /// <param name="unitOfWork">Unit of Work</param>
        /// <returns></returns>
        Task OnExecuteAsync(IDispatcher current, Lazy<IUnitOfWork> unitOfWork);
    }
}