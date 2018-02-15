using System.Data;

namespace Incoding.Core.Data
{

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null);
    }
}