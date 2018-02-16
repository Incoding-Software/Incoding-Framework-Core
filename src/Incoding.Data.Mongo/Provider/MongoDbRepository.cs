﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
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

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return database as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().InsertOne(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            throw new NotImplementedException();
            //GetCollection<TEntity>().InsertBatch(entities);
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void Flush() { }

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

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteOne(entity => entity.Id == id);
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteMany(entity => ids.Contains(entity.Id));
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            Delete<TEntity>(entity.TryGetValue("Id"));
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().DeleteMany(r => true);
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;
            return GetCollection<TEntity>().FindSync(entity => entity.Id == id).FirstOrDefault();
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return GetById<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
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