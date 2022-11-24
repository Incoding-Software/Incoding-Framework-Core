using System;
using FluentNHibernate.Utils;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Data;
using Incoding.WebTest30.Operations;

namespace Incoding.WebTest30.Models
{
    public class WhereSpecInterception : IRepositoryInterception
    {
        public Specification<TEntity> WhereSpec<TEntity>(Specification<TEntity> spec) where TEntity : class, IEntity, new()
        {
            if (typeof(TEntity).HasInterface(typeof(IName)))
            {
                spec = ValidSpec(spec);
            }

            return spec;
        }

        public Specification<TEntity> ValidSpec<TEntity>(Specification<TEntity> spec) where TEntity : class, IEntity, new()
        {
            var nameSpecType = typeof(NameSpec<>).MakeGenericType(typeof(TEntity));
            var nameSpec = Activator.CreateInstance(nameSpecType);
            //Specification<TEntity> nameSpec = (Specification<TEntity>)(object)new NameSpec<TEntity>();
            var newSpec = nameSpec as Specification<TEntity>;
            return spec == null ? newSpec : spec.And(newSpec);
        }
    }
}