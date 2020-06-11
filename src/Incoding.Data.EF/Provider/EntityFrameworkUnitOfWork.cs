using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Incoding.Data.EF.Provider
{
    [ExcludeFromCodeCoverage]
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase<DbContext>
    {
        #region Fields

        readonly IDbContextTransaction transaction;

        bool isWasCommit;

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWork(DbContext session, IsolationLevel level,bool isFlush)
                : base(session)
        {
            session.Database.AutoTransactionsEnabled = false;
            transaction = session.Database.BeginTransaction(level);
            
            if (!isFlush)
                session.ChangeTracker.AutoDetectChangesEnabled = false;
            repository = new EntityFrameworkRepository(session);
        }

        #endregion

        protected override void InternalFlush()
        {
            session.SaveChanges();
        }

        protected override async Task InternalFlushAsync()
        {
            await session.SaveChangesAsync();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
            isWasCommit = true;
        }

        protected override async Task InternalCommitAsync()
        {
            transaction.Commit();
            isWasCommit = true;
        }

        protected override void InternalSubmit()
        {
            if (!isWasCommit)
                transaction.Rollback();

            transaction.Dispose();
        }
    }
}