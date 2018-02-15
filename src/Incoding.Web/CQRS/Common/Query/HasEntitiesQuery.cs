using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;

namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block;

    #endregion

    public class HasEntitiesQuery<TEntity> : QueryBase<bool> where TEntity : class, IEntity, new()
    {
        protected override bool ExecuteResult()
        {
            return Repository.Query<TEntity>().Any();
        }
    }
}