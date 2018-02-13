using System.Data;

namespace Incoding.Data
{

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null);
    }
}