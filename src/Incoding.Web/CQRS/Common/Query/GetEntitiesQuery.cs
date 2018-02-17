using System.Collections.Generic;
using System.Linq;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;

namespace Incoding.Web.CQRS.Common.Query
{
    #region << Using >>

    #endregion

    public class GetEntitiesQuery<T> : QueryBase<List<T>> where T : class, IEntity, new()
    {
        #region Override

        protected override List<T> ExecuteResult()
        {
            return Repository.Query<T>().ToList();
        }

        #endregion
    }
}