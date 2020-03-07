using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Incoding.Data
{
    public interface IRepositoryInterception
    {
        Specification<TEntity> WhereSpec<TEntity>(Specification<TEntity> spec) where TEntity : class, IEntity, new();
    }
}