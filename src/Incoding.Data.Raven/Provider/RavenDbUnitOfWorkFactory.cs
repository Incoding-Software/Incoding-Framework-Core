using System.Data;
using Incoding.Core.Data;

namespace Incoding.Data.Raven.Provider
{

    public class RavenDbUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly IRavenDbSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public RavenDbUnitOfWorkFactory(IRavenDbSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion
        
        public IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null)
        {
            return new RavenDbUnitOfWork(sessionFactory.Open(connection), level);
        }
    }
}   