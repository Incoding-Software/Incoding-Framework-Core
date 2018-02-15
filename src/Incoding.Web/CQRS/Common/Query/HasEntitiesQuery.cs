namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block;
    using Incoding.Data;

    #endregion

    public class HasEntitiesQuery<TEntity> : QueryBase<bool> where TEntity : class, IEntity, new()
    {
        protected override bool ExecuteResult()
        {
            return Repository.Query<TEntity>().Any();
        }
    }
}