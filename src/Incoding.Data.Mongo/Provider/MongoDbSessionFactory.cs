using System;
using System.Collections.Concurrent;
using Incoding.Core.Data;
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
            var currentSession = client.GetDatabase(url.DatabaseName);
            return new MongoDatabaseWrapper(currentSession);
        }

        #endregion

    }
}