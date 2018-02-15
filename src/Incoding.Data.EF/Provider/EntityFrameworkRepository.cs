using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Core.Maybe;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data.EF.Provider
{
    #region << Using >>

    #endregion

    public class EntityFrameworkRepository : IRepository
    {
        #region Fields

        readonly DbContext session;

        #endregion

        #region Constructors

        public EntityFrameworkRepository(DbContext session)
        {
            this.session = session;
        }

        public EntityFrameworkRepository() { }

        #endregion

        #region IRepository Members

        public void ExecuteSql(string sql)
        {
            throw new NotImplementedException("Use GetProvider() to execute Raw queries");
            //using(DbConnection conn = new SqlCommand())session.Database.CurrentTransaction.;
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return session as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Set<TEntity>().Add(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            foreach (var entity in entities)
                Save(entity);
        }

        public void Flush()
        {
            //try
            //{
                session.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    foreach (var entry in ex.Entries)
            //    {
            //        if (entry.Entity is IEntity)
            //        {
            //            // Using a NoTracking query means we get the entity but it is not tracked by the context
            //            // and will not be merged with existing entities in the context.
            //            //var databaseEntity = session.Model.FindEntityType(entry.Entity.GetType()).People.AsNoTracking().Single(p => p.PersonId == ((Person)entry.Entity).PersonId));
            //            var databaseEntry = session.Entry(entry.Entity);

            //            foreach (var property in entry.Metadata.GetProperties())
            //            {
            //                var proposedValue = entry.Property(property.Name).CurrentValue;
            //                var originalValue = entry.Property(property.Name).OriginalValue;
            //                var databaseValue = databaseEntry.Property(property.Name).CurrentValue;

            //                // TODO: Logic to decide which value should be written to database
            //                 entry.Property(property.Name).CurrentValue = proposedValue;

            //                // Update original values to
            //                entry.Property(property.Name).OriginalValue = databaseEntry.Property(property.Name).CurrentValue;
            //            }
            //        }
            //        else
            //        {
            //            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
            //        }
            //    }

            //    // Retry the save operation
            //    session.SaveChanges();
            //}

        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (session.Entry(entity).State == EntityState.Detached)
                session.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(GetById<TEntity>(id));
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            ids.DoEach(r =>
            {
                var entity = session.Set<TEntity>().Find(r);
                session.Remove(entity);
            });
            //string idsAsString = ids.Select(o => o.GetType().IsAnyEquals(typeof(string), typeof(Guid)) ? "'{0}'".F(o.ToString()) : o.ToString()).AsString(",");
            //string queryString = "DELETE FROM {0} WHERE {1} IN ({2})".F(session.GetTableName<TEntity>(), "Id", idsAsString);
            //session.Database.ExecuteSqlCommand(queryString);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Set<TEntity>().Remove(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            session.Database.ExecuteSqlCommand("DELETE {0}".F(session.GetTableName<TEntity>()));
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            var dbSet = session.Set<TEntity>();
            var entity = dbSet.Find(id);
            return entity;
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().AsQueryable().Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().AsQueryable().Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public void Clear()
        {
            
        }

        #endregion
    }
}