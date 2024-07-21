using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;

namespace Incoding.WebTest80.Operations.Test
{
    public class CreateOrCloneEntity<T> : CommandBase<T> where T : IncEntityBase, new()
    {
        public int? Id { get; set; }

        
        //public Func<T> Create { get; set; }
       
        public Func<T, bool> IsEqual { get; set; }
        
        /// <inheritdoc />
        protected override T ExecuteResult()
        {
            //if (Id.HasValue && Id > 0)
            //{
            //    Result = Repository.LoadById<T>(Id);
            //    bool isEqual = (IsEqual?.Invoke(Result)).GetValueOrDefault();
            //    if (isEqual)
            //        return Result;

            //    //CloneExtended?.Invoke(Result, null);
            //}
            //else
            //{
            //    //Result = Create != null ? Create?.Invoke() : new T();
            //}
            
            return Result;
        }
    }
}