using System;
using System.Linq.Expressions;

namespace Incoding.Core.Extensions.LinqSpecs
{
    #region << Using >>

    #endregion

    /// <summary>
    /// Specification for Linq or Sql Provider filtering
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    [Serializable]
    public abstract class Specification<T>
    {
        #region Api Methods

        /// <summary>
        /// Specification Expression to check if satisfied by filter (Execute linq or translate to sql)
        /// </summary>
        /// <returns>Expression</returns>
        public abstract Expression<Func<T, bool>> IsSatisfiedBy();

        #endregion

        public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2);
        }

        public static bool operator false(Specification<T> spec1)
        {
            return false; // no-op. & and && do exactly the same thing.
        }

        public static bool operator true(Specification<T> spec1)
        {
            return false; // no - op. & and && do exactly the same thing.
        }

        public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
        {
            return new OrSpecification<T>(spec1, spec2);
        }

        public static Specification<T> operator !(Specification<T> spec1)
        {
            return new NegateSpecification<T>(spec1);
        }
    }
}