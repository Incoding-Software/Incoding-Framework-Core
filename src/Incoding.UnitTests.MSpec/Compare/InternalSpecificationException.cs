using Machine.Specifications;

namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

    #endregion

    public class InternalSpecificationException : SpecificationException
    {
        #region Constructors

        public InternalSpecificationException(string message)
                : base(message) { }

        #endregion
    }
}