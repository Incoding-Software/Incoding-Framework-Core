using Incoding.CQRS;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTaskCommandBase<T> : CommandBase
    {
        public T Item { get; set; }
        
        protected abstract override void Execute();
    }
}