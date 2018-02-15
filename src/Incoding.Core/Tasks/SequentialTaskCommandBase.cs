using Incoding.Core.CQRS.Core;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskCommandBase<T> : CommandBase
    {
        public T Item { get; set; }
        
        protected abstract override void Execute();
    }
}