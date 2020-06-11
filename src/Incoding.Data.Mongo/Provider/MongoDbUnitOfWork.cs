using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
        protected override async Task InternalFlushAsync()
        {
        }

        protected override void InternalCommit() { }
        protected override async Task InternalCommitAsync()
        {
        }
    }
}