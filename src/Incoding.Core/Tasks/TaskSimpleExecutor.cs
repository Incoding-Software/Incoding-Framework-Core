using System;
using System.Threading.Tasks;
using System.Timers;

namespace Incoding.Core.Tasks
{
    public class TaskSimpleExecutor : TaskExecutorBase
    {
        private Func<Task> _action;

        public TaskSimpleExecutor SetAction(Func<Task> action)
        {
            this._action = action;
            return this;
        }

        protected override async Task Execute()
        {
            if (StopImmediately)
                return;
            await _action();
        }
    }
}