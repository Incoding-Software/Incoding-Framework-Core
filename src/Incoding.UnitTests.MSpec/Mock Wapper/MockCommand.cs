using Incoding.Core.CQRS.Core;

namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

    #endregion

    public class MockCommand<TMessage> : MockMessage<TMessage, object> where TMessage : IMessage
    {
        #region Constructors

        protected MockCommand(TMessage instanceMessage)
                : base(instanceMessage) { }

        #endregion

        #region Factory method

        public static MockCommand<TMessage> When(TMessage instanceMessage)
        {
            return new MockCommand<TMessage>(instanceMessage);
        }
        
        #endregion

    }
}