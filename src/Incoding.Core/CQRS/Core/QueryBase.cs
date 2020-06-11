namespace Incoding.Core.CQRS.Core
{
    /// <summary>
    /// Base class for Query Message
    /// </summary>
    /// <typeparam name="TResult">Result</typeparam>
    public abstract class QueryBase<TResult> : MessageBase
    {
        #region Override

        /// <inheritdoc />
        protected override void Execute()
        {
            Result = ExecuteResult();
        }

        #endregion

        protected abstract TResult ExecuteResult();

    }
}