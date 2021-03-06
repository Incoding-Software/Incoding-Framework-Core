﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Core.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data.EF.Provider
{
    #region << Using >>

    #endregion

    public class AdHocFetchEFSpecification<TEntity> : AdHocFetchSpecificationBase<TEntity> where TEntity : class
    {
        public override AdHocFetchSpecificationBase<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> expression)
        {
            this.applies.Add(source => source.Include(expression));
            this.expressions.Add(expression);
            return this;
        }

        ////ncrunch: no coverage start
        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, object>> thenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> nextThenChild)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, object>> thenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }

        ////ncrunch: no coverage end        
    }
}