using MongoDB.Driver;

namespace Incoding.Data.Mongo.Provider
{
    public interface IMongoDbSessionFactory : ISessionFactory<MongoDatabaseWrapper> 
    { }
}