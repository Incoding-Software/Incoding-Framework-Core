using System;
using System.Threading.Tasks;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTask<T>
    {
        public Func<T, SequentialTaskCommandBase<T>> Command { get; set; }
        public Func<SequentialTaskQueryBase<T>> Query { get; set; }
        public Func<Task> AfterExecution { get; set; }
    }
}