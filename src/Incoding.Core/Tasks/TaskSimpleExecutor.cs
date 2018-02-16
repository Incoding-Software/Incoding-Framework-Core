using System;
using System.Timers;

namespace Incoding.Core.Tasks
{
    public class TaskSimpleExecutor : TaskExecutorBase
    {
        private Action _action;

        public TaskSimpleExecutor SetAction(Action action)
        {
            this._action = action;
            return this;
        }

        protected override void Execute()
        {
            if (StopImmediately)
                return;
            _action();
        }
    }
}