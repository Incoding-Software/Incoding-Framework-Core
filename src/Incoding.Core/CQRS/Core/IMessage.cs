using Incoding.Data;

namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using Newtonsoft.Json;

    #endregion

    public interface IMessage 
    {
        object Result { get; }
        
        MessageExecuteSetting Setting { get; set; }

        void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork);
    }
}