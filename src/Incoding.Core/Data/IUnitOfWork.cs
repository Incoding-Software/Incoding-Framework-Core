using System;
using System.Threading.Tasks;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public interface IUnitOfWork : IDisposable            
    {
        void Flush();
        Task FlushAsync();

        IRepository GetRepository();

        void Commit();
        Task CommitAsync();
    }
}