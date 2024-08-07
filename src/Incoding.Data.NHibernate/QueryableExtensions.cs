﻿using System.Linq;
using Incoding.Core;
using Incoding.Core.Block.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Data.NHibernate.Provider;

namespace Incoding.Data.NHibernate
{
    #region << Using >>

    #endregion

    public static class QueryableExtensions
    {
        public static IncPaginatedResult<TEntity> Paginated<TEntity>(this IQueryable<TEntity> source, OrderSpecification<TEntity> orderSpecification, Specification<TEntity> whereSpecification, FetchSpecification<TEntity> fetchSpecification, PaginatedSpecification paginatedSpecification) where TEntity : class, IEntity
        {
            int totalCount = source.Query(null, whereSpecification, null, null).Count();
            var paginatedItems = source.Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification).ToList();
            return new IncPaginatedResult<TEntity>(paginatedItems, totalCount);
        }

        public static IQueryable<TEntity> Query<TEntity>(this IQueryable<TEntity> source, OrderSpecification<TEntity> orderSpecification, Specification<TEntity> whereSpecification, FetchSpecification<TEntity> fetchSpecification, PaginatedSpecification paginatedSpecification) where TEntity : class, IEntity
        {
            if (whereSpecification.With(r => r.IsSatisfiedBy()) != null)
                source = source.Where(whereSpecification.IsSatisfiedBy());

            if (orderSpecification != null)
            {
                var order = new AdHocOrderSpecification<TEntity>();
                orderSpecification.SortedBy()(order);
                source = order.applies.Aggregate(source, (current, apply) => apply(current));
            }

            if (paginatedSpecification != null)
                source = source.Page(paginatedSpecification.CurrentPage, paginatedSpecification.PageSize);

            if (fetchSpecification != null)
            {
                AdHocFetchSpecificationBase<TEntity> fetch = null;
                    fetch = new AdHocFetchNhibernateSpecification<TEntity>();
                //else 
                //if (fullName.EqualsWithInvariant("System.Data.Entity.Internal.Linq.DbQueryProvider"))
                //    fetch = new AdHocFetchEFSpecification<TEntity>();
                //else if (fullName.Contains("Raven.Client.Linq.RavenQueryProvider"))
                //    fetch = new AdHocFetchRavenDbSpecification<TEntity>();

                //if (fetch != null)
                //{
                    fetchSpecification.FetchedBy()(fetch);
                    source = fetch.applies.Aggregate(source, (current, apply) => apply(current));
                //}
            }

            return source;
        }
    }
}