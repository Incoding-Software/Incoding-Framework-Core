using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.CQRS.Common
{
    #region << Using >>

    #endregion

    public interface IMessageInterception
    {
        Task OnBeforeAsync(IMessage message);

        Task OnAfterAsync(IMessage message);
    }
}