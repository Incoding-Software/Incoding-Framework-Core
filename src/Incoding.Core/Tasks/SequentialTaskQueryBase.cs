using System.Collections.Generic;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskQueryBase<T> : QueryBaseAsync<IEnumerable<T>>
    {
        protected abstract override Task<IEnumerable<T>> ExecuteResult();
    }
}