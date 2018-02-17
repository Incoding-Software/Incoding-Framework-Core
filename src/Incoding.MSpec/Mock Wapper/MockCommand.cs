using Incoding.Core.CQRS.Core;

namespace Incoding.MSpec
{
    #region << Using >>

    #endregion

    public class MockCommand<TMessage> : MockMessage<TMessage, object> where TMessage : CommandBase
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