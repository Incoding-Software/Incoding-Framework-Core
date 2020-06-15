using System;
using System.Threading.Tasks;
using Incoding.Core.Data;

namespace Incoding.Data
{
    public abstract class UnitOfWorkBase<TSession> : IUnitOfWork
            where TSession : class, IDisposable
    {
        #region Constructors

        protected UnitOfWorkBase(TSession session)
        {
            this.session = session;
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (!disposed)
            {
                InternalSubmit();
                session.Dispose();
            }

            disposed = true;
        }

        #endregion

        protected abstract void InternalSubmit();

        protected abstract void InternalFlush();
        protected abstract Task InternalFlushAsync();

        protected abstract void InternalCommit();
        protected abstract Task InternalCommitAsync();

        #region Fields

        protected readonly TSession session;

        protected IRepository repository;

        bool disposed;

        #endregion

        #region IUnitOfWork Members

        public IRepository GetRepository()
        {
            return repository;
        }

        public void Commit()
        {
            InternalCommit();
        }
        
        public async Task CommitAsync()
        {
            await InternalCommitAsync();
        }
        
        public void Flush()
        {
            if (!disposed)
                InternalFlush();
        }
        public async Task FlushAsync()
        {
            if (!disposed)
                await InternalFlushAsync();
        }

        #endregion
    }
}