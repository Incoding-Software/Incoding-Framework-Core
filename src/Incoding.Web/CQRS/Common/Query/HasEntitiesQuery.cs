using System.Linq;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;

namespace Incoding.Web.CQRS.Common.Query
{
    #region << Using >>

    #endregion

    public class HasEntitiesQuery<TEntity> : QueryBase<bool> where TEntity : class, IEntity, new()
    {
        protected override bool ExecuteResult()
        {
            return Repository.Query<TEntity>().Any();
        }
    }
}