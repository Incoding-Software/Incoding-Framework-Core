using System;
using MongoDB.Driver;

namespace Incoding.Data.Mongo.Provider
{
    public class MongoDatabaseWrapper : IDisposable
    {
        private IMongoDatabase currentSession;

        public MongoDatabaseWrapper(IMongoDatabase currentSession)
        {
            this.currentSession = currentSession;
        }

        public IMongoDatabase Instance => currentSession;

        public void Dispose()
        {
            // Mongo client responsible to manage connections
        }
    }
}