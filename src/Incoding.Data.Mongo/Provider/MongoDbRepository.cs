using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Incoding.Core.Block.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Core.Quality;
using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Incoding.Data.Mongo.Provider
{
    #region << Using >>

    #endregion

    public class MongoDbRepository : IRepository
    {
        #region Fields

        readonly MongoDatabaseWrapper database;

        #endregion

        #region Constructors

        static readonly List<Func<IRepositoryInterception>> interceptions = new List<Func<IRepositoryInterception>>();

        public static void SetInterception(Func<IRepositoryInterception> create)
        {
            interceptions.Add(create);
        }

        public MongoDbRepository(MongoDatabaseWrapper database)
        {
            this.database = database;
        }

        #endregion

        #region IRepository Members

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void ExecuteSql(string sql)
        {
            throw new NotSupportedException();
        }
        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public async Task ExecuteSqlAsync(string sql)
        {
            throw new NotSupportedException();
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return database as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().InsertOne(entity);
        }

        public async Task SaveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            await GetCollection<TEntity>().InsertOneAsync(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().InsertMany(entities);
        }

        public async Task SavesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            await GetCollection<TEntity>().InsertManyAsync(entities);
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void Flush() { }
        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public async Task FlushAsync() { }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            var id = entity.TryGetValue("Id");
            if (GetById<TEntity>(id) == null)
            {
                Save(entity);
                return;
            }

            var update = new UpdateDefinitionBuilder<TEntity>();
            foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(r => !r.Name.EqualsWithInvariant("Id")))
            {
                var value = property.GetValue(entity, null);

                BsonValue bsonValue = BsonNull.Value;
                if (value != null)
                {
                    var type = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                       ? property.PropertyType.GetGenericArguments()[0]
                                       : property.PropertyType;

                    if (type == typeof(string))
                        bsonValue = new BsonString(value.ToString());
                    else if (type == typeof(bool))
                        bsonValue = new BsonBoolean((bool)value);
                    else if (type == typeof(DateTime))
                        bsonValue = new BsonDateTime((DateTime)value);
                    else if (type == typeof(long))
                        bsonValue = new BsonInt64((long)value);
                    else if (type == typeof(int))
                        bsonValue = new BsonInt32((int)value);
                    else if (type == typeof(byte[]))
                        bsonValue = new BsonBinaryData((byte[])value);
                    else if (type == typeof(Guid))
                        bsonValue = new BsonBinaryData((Guid)value);
                    else if (type.IsEnum)
                        bsonValue = new BsonString(value.ToString());
                    else if (type.IsImplement<IEnumerable>())
                        bsonValue = new BsonArray(value as IEnumerable);
                    else if (type.IsClass && type.IsImplement<IEntity>())
                        bsonValue = new BsonDocumentWrapper(value);
                    else
                        throw new ArgumentOutOfRangeException("propertyType {0} does not bson value".F(type));
                }

                update.Set(property.Name, bsonValue);
            }

            GetCollection<TEntity>().UpdateMany(r => r.Id == id, update.Combine());
        }

        public async Task SaveOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            var id = entity.TryGetValue("Id");
            if (GetById<TEntity>(id) == null)
            {
                await SaveAsync(entity);
                return;
            }

            var update = new UpdateDefinitionBuilder<TEntity>();
            foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(r => !r.Name.EqualsWithInvariant("Id")))
            {
                var value = property.GetValue(entity, null);

                BsonValue bsonValue = BsonNull.Value;
                if (value != null)
                {
                    var type = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                       ? property.PropertyType.GetGenericArguments()[0]
                                       : property.PropertyType;

                    if (type == typeof(string))
                        bsonValue = new BsonString(value.ToString());
                    else if (type == typeof(bool))
                        bsonValue = new BsonBoolean((bool)value);
                    else if (type == typeof(DateTime))
                        bsonValue = new BsonDateTime((DateTime)value);
                    else if (type == typeof(long))
                        bsonValue = new BsonInt64((long)value);
                    else if (type == typeof(int))
                        bsonValue = new BsonInt32((int)value);
                    else if (type == typeof(byte[]))
                        bsonValue = new BsonBinaryData((byte[])value);
                    else if (type == typeof(Guid))
                        bsonValue = new BsonBinaryData((Guid)value);
                    else if (type.IsEnum)
                        bsonValue = new BsonString(value.ToString());
                    else if (type.IsImplement<IEnumerable>())
                        bsonValue = new BsonArray(value as IEnumerable);
                    else if (type.IsClass && type.IsImplement<IEntity>())
                        bsonValue = new BsonDocumentWrapper(value);
                    else
                        throw new ArgumentOutOfRangeException("propertyType {0} does not bson value".F(type));
                }

                update.Set(property.Name, bsonValue);
            }

            await GetCollection<TEntity>().UpdateManyAsync(r => r.Id == id, update.Combine());
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteOne(entity => entity.Id == id);
        }

        public async Task DeleteAsync<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            await GetCollection<TEntity>().DeleteOneAsync(entity => entity.Id == id);
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteMany(entity => ids.Contains(entity.Id));
        }

        public async Task DeleteByIdsAsync<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            await GetCollection<TEntity>().DeleteManyAsync(entity => ids.Contains(entity.Id));
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            Delete<TEntity>(entity.TryGetValue("Id"));
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            await DeleteAsync<TEntity>(entity.TryGetValue("Id"));
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteMany(r => true);
        }
        public async Task DeleteAllAsync<TEntity>() where TEntity : class, IEntity, new()
        {
            await GetCollection<TEntity>().DeleteManyAsync(r => true);
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;
            return GetCollection<TEntity>().FindSync(entity => entity.Id == id).FirstOrDefault();
        }
        public async Task<TEntity> GetByIdAsync<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;
            var result = await GetCollection<TEntity>().FindAsync(entity => entity.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return GetById<TEntity>(id);
        }

        public async Task<TEntity> LoadByIdAsync<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return await GetByIdAsync<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new()
        {
            Specification<TEntity> where = whereSpecification;
            if (!skipInterceptions)
                foreach (var interception in interceptions)
                {
                    where = interception().WhereSpec(where);
                }
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Query(orderSpecification, where, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new()
        {
            Specification<TEntity> where = whereSpecification;
            if (!skipInterceptions)
                foreach (var interception in interceptions)
                {
                    where = interception().WhereSpec(where);
                }
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Paginated(orderSpecification, where, fetchSpecification, paginatedSpecification);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return database.Instance.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}