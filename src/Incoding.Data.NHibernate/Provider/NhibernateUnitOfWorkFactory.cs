using System.Data;
using Incoding.Core.Data;

namespace Incoding.Data.NHibernate.Provider
{
    #region << Using >>

    #endregion

    public class NhibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly INhibernateSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public NhibernateUnitOfWorkFactory(INhibernateSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null)
        {
            return new NhibernateUnitOfWork(sessionFactory.Open(connection), level, isFlush);
        }

        #endregion
    }
}