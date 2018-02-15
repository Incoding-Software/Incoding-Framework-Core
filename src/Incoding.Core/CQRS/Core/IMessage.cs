using System;
using Incoding.Core.Data;

namespace Incoding.Core.CQRS.Core
{
    #region << Using >>

    #endregion

    public interface IMessage 
    {
        object Result { get; }
        
        MessageExecuteSetting Setting { get; set; }

        void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork);
    }
}