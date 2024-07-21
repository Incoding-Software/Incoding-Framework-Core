using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskCommandBase<T> : CommandBaseAsync
    {
        public T Item { get; set; }
        protected abstract override Task ExecuteAsync();
    }
}