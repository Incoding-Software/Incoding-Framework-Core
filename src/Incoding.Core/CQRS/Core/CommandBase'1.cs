using System.Runtime.Serialization;
using Incoding.Core.Quality;
using Newtonsoft.Json;

namespace Incoding.Core.CQRS.Core
{
    public abstract class CommandBase<T> : CommandBase
    {
        [IgnoreCompare("Design fixed")]
        [JsonIgnore]
        [IgnoreDataMember]
        public new virtual T Result { get; protected set; }

        protected sealed override void Execute()
        {
            Result = ExecuteResult();
        }

        protected abstract T ExecuteResult();
    }
}