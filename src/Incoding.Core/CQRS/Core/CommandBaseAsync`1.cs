using System.Runtime.Serialization;
using System.Threading.Tasks;
using Incoding.Core.Quality;
using Newtonsoft.Json;

namespace Incoding.Core.CQRS.Core
{
    /// <summary>
    /// Async Command with Typed Result
    /// </summary>
    /// <typeparam name="T">Result</typeparam>
    public abstract class CommandBaseAsync<T> : CommandBaseAsync
    {
        [IgnoreCompare("Design fixed")]
        [JsonIgnore]
        [IgnoreDataMember]
        public new virtual T Result { get; protected set; }

        protected sealed override async Task ExecuteAsync()
        {
            Result = await ExecuteResult();
            base.Result = Result;
        }

        /// <summary>
        /// Execute Command
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> ExecuteResult();
    }
}