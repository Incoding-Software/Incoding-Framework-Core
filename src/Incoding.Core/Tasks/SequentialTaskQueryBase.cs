using System.Collections.Generic;
using Incoding.CQRS;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskQueryBase<T> : QueryBase<IEnumerable<T>>
    {
        protected abstract override IEnumerable<T> ExecuteResult();
    }
}