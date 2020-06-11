using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Incoding.Core.Data;
using Incoding.Core.Quality;
using Newtonsoft.Json;

namespace Incoding.Core.CQRS.Core
{
    #region << Using >>

    #endregion

    /// <summary>
    /// Base Async Message class
    /// </summary>
    public abstract class MessageBaseAsync : IMessage
    {
        #region Fields

        Lazy<IRepository> lazyRepository;

        IDispatcher messageDispatcher;

        #endregion

        #region Properties

        [IgnoreCompare("System"), JsonIgnore, IgnoreDataMember]
        protected IRepository Repository { get { return lazyRepository.Value; } }

        [IgnoreCompare("System"), JsonIgnore, IgnoreDataMember]
        protected IDispatcher Dispatcher { get { return messageDispatcher; } }


        #endregion

        #region IMessage<TResult> Members

        /// <inheritdoc />
        [IgnoreCompare("Design fixed"), JsonIgnore, IgnoreDataMember]
        public virtual object Result { get; protected set; }

        /// <inheritdoc />
        [IgnoreCompare("Design fixed"), IgnoreDataMember]
        public virtual MessageExecuteSetting Setting { get; set; }

        /// <inheritdoc />
        public virtual void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork)
        {
            Result = null;
            lazyRepository = new Lazy<IRepository>(() => unitOfWork.Value.GetRepository());
            messageDispatcher = current;
            Task.Run(ExecuteAsync).Wait(TimeSpan.MaxValue);
        }

        /// <inheritdoc />
        public async Task OnExecuteAsync(IDispatcher current, Lazy<IUnitOfWork> unitOfWork)
        {
            Result = null;
            lazyRepository = new Lazy<IRepository>(() => unitOfWork.Value.GetRepository());
            messageDispatcher = current;
            await ExecuteAsync();
        }

        #endregion

        #region Api Methods

        protected abstract Task ExecuteAsync();

        #endregion


    }
}