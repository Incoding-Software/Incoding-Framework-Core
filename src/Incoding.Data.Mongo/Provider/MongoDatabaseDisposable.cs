using System;
using MongoDB.Driver;

namespace Incoding.Data.Mongo.Provider
{
    #region << Using >>

    #endregion

    public class MongoDatabaseDisposable : IDisposable
    {
        #region Fields

        readonly MongoDatabase instance;

        #endregion

        #region Constructors

        public MongoDatabaseDisposable(MongoDatabase instance)
        {
            this.instance = instance;
        }

        #endregion

        #region Properties

        public MongoDatabase Instance { get { return this.instance; } }

        #endregion

        #region Disposable

        public void Dispose() { }

        #endregion
    }
}