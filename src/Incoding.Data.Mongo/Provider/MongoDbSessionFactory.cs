using System;
using System.Collections.Concurrent;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Incoding.Data.Mongo.Provider
{
    #region << Using >>

    #endregion

    public class MongoDbSessionFactory : IMongoDbSessionFactory
    {
        #region Static Fields

        static readonly ConcurrentDictionary<string, MongoClient> clients = new ConcurrentDictionary<string, MongoClient>();

        [ThreadStatic]
        static IMongoDatabase currentSession;

        #endregion

        #region Fields

        readonly string defaultUrl;

        #endregion

        #region Constructors

        static MongoDbSessionFactory()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(IncEntityBase)))
                BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
        }

        public MongoDbSessionFactory(string defaultUrl)
        {
            this.defaultUrl = defaultUrl;
            clients.AddOrUpdate(defaultUrl, s => new MongoClient(new MongoUrl(defaultUrl)), (s, server) => server);
        }

        #endregion

        #region IMongoDbSessionFactory Members
        
        public MongoDatabaseWrapper Open(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                connectionString = this.defaultUrl;

            var url = new MongoUrl(connectionString);
            var client = clients.GetOrAdd(connectionString, s => new MongoClient(url));
            currentSession = client.GetDatabase(url.DatabaseName);
            return new MongoDatabaseWrapper(currentSession);
        }

        #endregion

    }
}