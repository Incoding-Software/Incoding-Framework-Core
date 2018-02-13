using System.Data;
using Raven.Client;

namespace Incoding.Data.Raven.Provider
{

    public class RavenDbUnitOfWork : UnitOfWorkBase<IDocumentSession>
    {
        #region Fields

        readonly System.Transactions.TransactionScope transaction;

        #endregion

        #region Constructors

        public RavenDbUnitOfWork(IDocumentSession session,IsolationLevel level)
                : base(session)
        {
            transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew);
            repository = new RavenDbRepository(session);
        }

        #endregion

        protected override void InternalFlush()
        {
            session.SaveChanges();
        }

        protected override void InternalCommit()
        {
            transaction.Complete();
        }

        protected override void InternalSubmit()
        {
            transaction.Dispose();
        }
    }
}