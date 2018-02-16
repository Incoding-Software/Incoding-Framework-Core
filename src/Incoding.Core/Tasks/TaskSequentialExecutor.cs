using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incoding.Core.CQRS;

namespace Incoding.Core.Tasks
{
    public class TaskSequentialExecutor<TItem> : TaskExecutorBase
    {
        private Func<SequentialTaskQueryBase<TItem>> _query;
        private readonly Func<TItem, SequentialTaskCommandBase<TItem>> _createCommand;
        
        public TaskSequentialExecutor(Func<SequentialTaskQueryBase<TItem>> query, Func<TItem, SequentialTaskCommandBase<TItem>> createCommand)
        {
            _query = query;
            _createCommand = createCommand;
        }

        protected override void Execute()
        {
            IEnumerable<TItem> items = Dispatcher.Query(_query());
            if (StopImmediately)
                return;
            foreach (var item in items)
            {
                var cmd = _createCommand(item);
                cmd.Item = item;
                Dispatcher.Push(cmd);
                if (StopImmediately)
                    return;
                Task.Delay(Options.DelayBetweenSequencesMs);
            }

            Options.AfterExecution?.Invoke();
        }
    }
}