using Incoding.Core.CQRS.Core;

namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

    #endregion

    public class MockQuery<TMessage, TResult> : MockMessage<TMessage, TResult> where TMessage : IMessage
    {
        #region Constructors

        protected MockQuery(TMessage instanceMessage)
                : base(instanceMessage) { }

        #endregion

        #region Factory constructors

        public static MockMessage<TMessage, TResult> When(TMessage instanceMessage)
        {
            return new MockQuery<TMessage, TResult>(instanceMessage);
        }

        #endregion
    }
}