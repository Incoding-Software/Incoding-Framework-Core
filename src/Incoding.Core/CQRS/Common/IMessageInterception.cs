using Incoding.Core.CQRS.Core;

namespace Incoding.Core.CQRS.Common
{
    #region << Using >>

    #endregion

    public interface IMessageInterception
    {
        void OnBefore(IMessage message);

        void OnAfter(IMessage message);
    }
}