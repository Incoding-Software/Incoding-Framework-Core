using Incoding.CQRS;

namespace Incoding.Mvc.MvcContrib.MVD
{
    #region << Using >>

    #endregion

    public interface IMessageInterception
    {
        void OnBefore(IMessage message);

        void OnAfter(IMessage message);
    }
}