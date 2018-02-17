using System.Data;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using MongoDB.Driver;

namespace Incoding.Data.Mongo.Provider
{
    #region << Using >>

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class MongoDbUnitOfWork : UnitOfWorkBase<MongoDatabaseWrapper>
    {
        #region Constructors

        public MongoDbUnitOfWork(MongoDatabaseWrapper session, IsolationLevel level)
                : base(session)
        {
            repository = new MongoDbRepository(session);
        }

        #endregion

        protected override void InternalSubmit() { }

        protected override void InternalFlush() { }

        protected override void InternalCommit() { }
    }
}