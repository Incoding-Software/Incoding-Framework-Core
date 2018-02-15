namespace Incoding.Core.CQRS.Core
{
    public abstract class QueryBase<TResult> : MessageBase
    {
        #region Override

        protected override void Execute()
        {
            Result = ExecuteResult();
        }

        #endregion

        protected abstract TResult ExecuteResult();

    }
}