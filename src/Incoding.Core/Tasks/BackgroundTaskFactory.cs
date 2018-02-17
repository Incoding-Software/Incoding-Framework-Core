using System;
using System.Collections.Concurrent;
using Incoding.Core;

namespace Incoding.Core.Tasks
{
    public class BackgroundTaskFactory
    {
        bool initialized;

        static readonly Lazy<BackgroundTaskFactory> instance = new Lazy<BackgroundTaskFactory>(() => new BackgroundTaskFactory());

        public static BackgroundTaskFactory Instance { get { return instance.Value; } }

        public ConcurrentDictionary<string, TaskExecutorBase> Tasks = new ConcurrentDictionary<string, TaskExecutorBase>();

        public void Initialize()
        {
            initialized = true;
            Tasks.DoEach(pair => pair.Value.Start());
        }

        public void StopAll()
        {
            foreach (var taskExecutor in Tasks)
            {
                taskExecutor.Value.Stop(true);
            }
        }

        public bool IsInitialized { get { return initialized; } }

        public TaskSimpleExecutor AddExecutor(string key, Action action, Action<TaskExecutorBase.TaskExecutorOptions> executorOptions = null)
        {
            var taskExecutor = new TaskSimpleExecutor().SetAction(action).SetOptions(executorOptions);
            if (Tasks.TryAdd(key, taskExecutor))
                return taskExecutor as TaskSimpleExecutor;
            return null;
        }

        public TaskSequentialExecutor<TItem> AddSequentalExecutor<TItem>(string key, Func<SequentialTaskQueryBase<TItem>> query, Func<TItem, SequentialTaskCommandBase<TItem>> createCommand, Action<TaskExecutorBase.TaskExecutorOptions> executorOptions = null)
        {
            var taskExecutor = new TaskSequentialExecutor<TItem>(query, createCommand).SetOptions(executorOptions);
            if (Tasks.TryAdd(key, taskExecutor))
                return taskExecutor as TaskSequentialExecutor<TItem>;
            return null;
        }

        public TaskSequentialExecutor<TItem> AddSequentalExecutor<TItem>(string key, SequentialTask<TItem> task, Action<TaskExecutorBase.TaskExecutorOptions> executorOptions = null)
        {
            var taskExecutor = new TaskSequentialExecutor<TItem>(task.Query, task.Command).SetOptions(executorOptions);
            if (Tasks.TryAdd(key, taskExecutor))
                return taskExecutor as TaskSequentialExecutor<TItem>;
            return null;
        }
    }
}