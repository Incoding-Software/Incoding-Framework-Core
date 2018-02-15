using System.Collections.Generic;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskQueryBase<T> : QueryBase<IEnumerable<T>>
    {
        protected abstract override IEnumerable<T> ExecuteResult();
    }
}