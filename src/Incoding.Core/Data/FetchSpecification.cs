using System;
using Incoding.Core.Extensions;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public abstract class FetchSpecification<TEntity> where TEntity : class
    {
        #region Api Methods
        
        public abstract Action<AdHocFetchSpecificationBase<TEntity>> FetchedBy();

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return Equals(obj as FetchSpecification<TEntity>);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        protected bool Equals(FetchSpecification<TEntity> other)
        {
            ////ncrunch: no coverage start
            if (!this.IsReferenceEquals(other))
                return false;

            ////ncrunch: no coverage end
            /// 
            return true;
        }

        #endregion
    }
}