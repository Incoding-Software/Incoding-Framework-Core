using System;

namespace Incoding.Core.Tasks
{
    public abstract class SequentialTask<T>
    {
        public abstract Func<T, SequentialTaskCommandBase<T>> Command { get; set; }
        public abstract Func<SequentialTaskQueryBase<T>> Query { get; set; }
        public abstract Action AfterExecution { get; set; }
    }
}