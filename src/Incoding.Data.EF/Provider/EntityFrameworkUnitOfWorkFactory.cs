using System.Diagnostics.CodeAnalysis;
using System.Data;
using Incoding.Core.Data;
using JetBrains.Annotations;

namespace Incoding.Data.EF.Provider
{
    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly IEntityFrameworkSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWorkFactory(IEntityFrameworkSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null)
        {
            return new EntityFrameworkUnitOfWork(sessionFactory.Open(connection), level,isFlush);
        }

        #endregion
    }
}