using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core.Block.Core;
using Incoding.Core.Extensions.LinqSpecs;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public interface IRepository
    {
        #region Methods

        /// <summary>
        /// Execute raw SQL code
        /// </summary>
        /// <param name="sql">Sql string</param>
        void ExecuteSql(string sql);
        /// <summary>
        /// Execute raw SQL code
        /// </summary>
        /// <param name="sql">Sql string</param>
        Task ExecuteSqlAsync(string sql);

        TProvider GetProvider<TProvider>() where TProvider:class;
        
        /// <summary>
        ///     Persist the given entity instance
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new();
        /// <summary>
        ///     Persist the given entity instance
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        Task SaveAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Persist the given entity instance
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="entities">Entities instance</param>
        void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new();
        /// <summary>
        ///     Persist the given entity instance
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="entities">Entities instance</param>
        Task SavesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Flushed if a query is requested for some entity type and there are dirty local entity instances
        /// </summary>
        void Flush();
        /// <summary>
        ///     Flushed if a query is requested for some entity type and there are dirty local entity instances
        /// </summary>
        Task FlushAsync();

        /// <summary>
        ///     <see cref="Saves{TEntity}" /> or update
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        ///     <see cref="SavesAsync{TEntity}" /> or update
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        Task SaveOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete a entity from the datastore  by id
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="id">Id</param>
        void Delete<TEntity>(object id) where TEntity : class, IEntity, new();
        /// <summary>
        ///     Delete a entity from the datastore  by id
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="id">Id</param>
        Task DeleteAsync<TEntity>(object id) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete a entity from the datastore  by ids ( don't support cascade )
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="ids">Ids</param>
        void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete a entity from the datastore  by ids ( don't support cascade )
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="ids">Ids</param>
        Task DeleteByIdsAsync<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete a entity instance from the datastore
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Persistence instance</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete a entity instance from the datastore
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Persistence instance</param>
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete all entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        void DeleteAll<TEntity>() where TEntity : class, IEntity, new();

        /// <summary>
        ///     Delete all entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        Task DeleteAllAsync<TEntity>() where TEntity : class, IEntity, new();

        /// <summary>
        ///     Getting entity instance from persist
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns> Instance entity </returns>
        TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Getting entity instance from persist
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns> Instance entity </returns>
        Task<TEntity> GetByIdAsync<TEntity>(object id) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Getting entity instance from persist or cache
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns> Instance entity </returns>
        TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Getting entity instance from persist or cache
        /// </summary>
        /// <typeparam name="TEntity">Strong type entity</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns> Instance entity </returns>
        Task<TEntity> LoadByIdAsync<TEntity>(object id) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Query entities with specifications
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Strong type entity
        /// </typeparam>
        /// <param name="orderSpecification">
        ///     Specification how sort entities
        /// </param>
        /// <param name="whereSpecification">
        ///     Specification how filter entities
        /// </param>
        /// <param name="fetchSpecification">
        ///     Specification why join with entities ( many to many , many to one , one to one )
        /// </param>
        /// <param name="paginatedSpecification">
        ///     Specification how much skip and take entities
        /// </param>
        /// <returns>
        ///     Queryable collections ( pending request )
        /// </returns>
        IQueryable<TEntity> Query<TEntity>(Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new();

        /// <summary>
        ///     Query page by page
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Strong type entity
        /// </typeparam>
        /// <param name="paginatedSpecification">
        ///     Specification how much skip and take entities
        /// </param>
        /// <param name="orderSpecification">
        ///     Specification how sort entities
        /// </param>
        /// <param name="whereSpecification">
        ///     Specification how filter entities
        /// </param>
        /// <param name="fetchSpecification">
        ///     Specification why join with entities ( many to many , many to one , one to one )
        /// </param>
        /// <returns>
        ///     <see cref="IncPaginatedResult{TItem}" />
        /// </returns>
        IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, Specification<TEntity> whereSpecification = null, OrderSpecification<TEntity> orderSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new();

        /// <summary>
        /// Completely clear the session. Evict all loaded instances and cancel all pending
        /// </summary>
        void Clear();

        #endregion
    }
}