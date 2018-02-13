using System.Data;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Incoding.Data.Mongo.Provider
{
    #region << Using >>

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class MongoDbUnitOfWork : UnitOfWorkBase<MongoDatabaseDisposable>
    {
        #region Constructors

        public MongoDbUnitOfWork(MongoDatabaseDisposable session, IsolationLevel level)
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