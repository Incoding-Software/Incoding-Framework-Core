using System.Data;
using System.Threading.Tasks;
using NHibernate;

namespace Incoding.Data.NHibernate
{
    #region << Using >>

    #endregion

    public class NhibernateUnitOfWork : UnitOfWorkBase<ISession>
    {
        #region Fields

        readonly ITransaction transaction;

        #endregion

        #region Constructors

        public NhibernateUnitOfWork(ISession session, IsolationLevel isolationLevel, bool isFlush)
                : base(session)
        {
            transaction = session.BeginTransaction(isolationLevel);
            bool isReadonly = !isFlush;
            session.DefaultReadOnly = isReadonly;
            if (isReadonly)
                session.FlushMode = FlushMode.Manual;
            repository = new NhibernateRepository(session);
        }

        #endregion

        protected override void InternalFlush()
        {
            session.Flush();
        }
        protected override async Task InternalFlushAsync()
        {
            await session.FlushAsync();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
        }
        protected override async Task InternalCommitAsync()
        {
            await transaction.CommitAsync();
        }

        protected override void InternalSubmit()
        {
            //if(session.Connection != null && session.Connection.State != ConnectionState.Closed)
            //    session.Connection.Close();
            transaction.Dispose();
        }
    }
}