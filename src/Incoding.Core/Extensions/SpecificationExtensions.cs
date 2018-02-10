using Incoding.Data;

namespace Incoding.Extensions
{
    #region << Using >>

    #endregion

    public static class SpecificationExtensions
    {
        #region Factory constructors

        public static Specification<T> And<T>(this Specification<T> first, Specification<T> second)
        {
            return first & second;
        }

        public static FetchSpecification<T> And<T>(this FetchSpecification<T> first, FetchSpecification<T> second) where T: class
        {
            return new AndFetchSpecification<T>(first, second);
        }

        public static Specification<T> Or<T>(this Specification<T> first, Specification<T> second)
        {
            return first | second;
        }

        #endregion
    }
}