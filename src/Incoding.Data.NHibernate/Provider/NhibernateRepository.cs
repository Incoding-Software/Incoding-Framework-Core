using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Incoding.Core.Block.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using JetBrains.Annotations;
using NHibernate;
using NHibernate.Persister.Entity;

namespace Incoding.Data.NHibernate
{
    #region << Using >>

    #endregion

    public class NhibernateRepository : IRepository
    {
        #region Fields

        readonly ISession session;

        #endregion

        #region Constructors

        static readonly List<Func<IRepositoryInterception>> interceptions = new List<Func<IRepositoryInterception>>();

        public static void SetInterception(Func<IRepositoryInterception> create)
        {
            interceptions.Add(create);
        }

        public NhibernateRepository(ISession session)
        {
            this.session = session;
        }

        [Obsolete("Not needed use Repository on IOC", true), ExcludeFromCodeCoverage, UsedImplicitly]
        public NhibernateRepository() { }

        #endregion

        #region IRepository Members

        public void ExecuteSql(string sql)
        {
            session.CreateSQLQuery(sql).ExecuteUpdate();
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return session as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Save(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            foreach (var entity in entities)
                Save(entity);
        }

        public void Flush()
        {
            session.Flush();
        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.SaveOrUpdate(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(session.Load<TEntity>(id));
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            var metadata = GetMetaData<TEntity>();
            string idColumnName = metadata.GetPropertyColumnNames("Id").FirstOrDefault();
            string tableName = metadata.TableName;
            string queryString = "DELETE FROM [{0}] WHERE {1} IN ({2})".F(tableName, idColumnName, ids.Select(o => o.GetType().IsAnyEquals(typeof(string), typeof(Guid)) ? "'{0}'".F(o.ToString()) : o.ToString()).AsString(","));
            session
                    .CreateSQLQuery(queryString)
                    .ExecuteUpdate();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Delete(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            session.CreateSQLQuery("DELETE {0}".F(GetMetaData<TEntity>().TableName))
                   .ExecuteUpdate();
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;

            return session.Get<TEntity>(id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;

            return session.Load<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new()
        {
            Specification<TEntity> where = whereSpecification;
            if (!skipInterceptions)
                foreach (var interception in interceptions)
                {
                    where = interception().WhereSpec(where);
                }
            return session.Query<TEntity>().Query(orderSpecification, where, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new()
        {
            Specification<TEntity> where = whereSpecification;
            if (!skipInterceptions)
                foreach (var interception in interceptions)
                {
                    where = interception().WhereSpec(where);
                }
            return session.Query<TEntity>().Paginated(orderSpecification, where, fetchSpecification, paginatedSpecification);
        }


        public void Clear()
        {
            this.session.Clear();
        }

        #endregion

        SingleTableEntityPersister GetMetaData<T>()
        {
            var metadata = session.SessionFactory.GetClassMetadata(typeof(T));
            return (SingleTableEntityPersister)metadata;
        }
    }
}