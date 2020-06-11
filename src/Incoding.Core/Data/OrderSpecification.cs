using System;
using Incoding.Core.Extensions;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public abstract class OrderSpecification<TEntity>
    {
        #region Api Methods
        /// <summary>
        /// Action to provide sorting Specification for linq or sql provider
        /// </summary>
        /// <returns>Order Specification expression action</returns>
        public abstract Action<AdHocOrderSpecification<TEntity>> SortedBy();

        #endregion

        #region Equals


        public override bool Equals(object obj)
        {
            return Equals(obj as OrderSpecification<TEntity>);
        }

        ////ncrunch: no coverage start
        public override int GetHashCode()
        {
            return 0;
        }

        ////ncrunch: no coverage end
        protected bool Equals(OrderSpecification<TEntity> other)
        {
            ////ncrunch: no coverage start
            if (!this.IsReferenceEquals(other))
                return false;

            ////ncrunch: no coverage end
            var leftOrder = new AdHocOrderSpecification<TEntity>();
            SortedBy()(leftOrder);

            var rightOrder = new AdHocOrderSpecification<TEntity>();
            other.SortedBy()(rightOrder);

            return leftOrder.Equals(rightOrder);
        }

        #endregion
    }
}