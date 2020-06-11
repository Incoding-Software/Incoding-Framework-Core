using System.Threading.Tasks;

namespace Incoding.Core.CQRS.Core
{
    /// <summary>
    /// Async Query
    /// </summary>
    /// <typeparam name="TResult">Result</typeparam>
    public abstract class QueryBaseAsync<TResult> : MessageBaseAsync
    {
        #region Override

        /// <inheritdoc />
        protected override async Task ExecuteAsync()
        {
            Result = await ExecuteResult();
        }

        #endregion

        /// <summary>
        /// Execute Result
        /// </summary>
        /// <returns>Result</returns>
        protected abstract Task<TResult> ExecuteResult();

    }
}