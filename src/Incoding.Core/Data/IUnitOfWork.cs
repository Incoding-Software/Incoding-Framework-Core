using System;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public interface IUnitOfWork : IDisposable            
    {
        void Flush();

        IRepository GetRepository();

        void Commit();
    }
}