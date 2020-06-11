using System.Runtime.Serialization;
using Incoding.Core.Quality;
using Newtonsoft.Json;

namespace Incoding.Core.CQRS.Core
{
    /// <summary>
    /// Command Typed class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandBase<T> : CommandBase
    {
        [IgnoreCompare("Design fixed")]
        [JsonIgnore]
        [IgnoreDataMember]
        public new virtual T Result { get; protected set; }

        protected sealed override void Execute()
        {
            Result = ExecuteResult();
            base.Result = Result;
        }

        /// <summary>
        /// Execute Command
        /// </summary>
        /// <returns>Result</returns>
        protected abstract T ExecuteResult();
    }
}