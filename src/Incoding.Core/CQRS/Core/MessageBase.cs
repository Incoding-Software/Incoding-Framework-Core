﻿using System;
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
    /// Base Message class
    /// </summary>
    public abstract class MessageBase : IMessage
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
            Execute();
        }

        /// <inheritdoc />
        public async Task OnExecuteAsync(IDispatcher current, Lazy<IUnitOfWork> unitOfWork)
        {
            OnExecute(current, unitOfWork);
        }

        #endregion

        #region Api Methods

        /// <summary>
        /// Execute Message function
        /// </summary>
        protected abstract void Execute();

        #endregion


    }
}