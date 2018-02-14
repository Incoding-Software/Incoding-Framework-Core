﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Incoding.Block.Logging;
using Incoding.Core.Tasks;
using Incoding.Maybe;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;

namespace Incoding.Web.Tasks
{
    public class BackendTaskFactory
    {
        private readonly IApplicationLifetime _applicationLifetime;

        IApplicationLifetime appLifetime;
        bool initialized;

        static readonly Lazy<BackendTaskFactory> instance = new Lazy<BackendTaskFactory>(() => new BackendTaskFactory());

        public static BackendTaskFactory Instance { get { return instance.Value; } }

        public ConcurrentDictionary<string, TaskExecutorBase> Tasks = new ConcurrentDictionary<string, TaskExecutorBase>();

        public void Initialize([NotNull] IApplicationLifetime applicationLifetime)
        {
            this.appLifetime = applicationLifetime;

            appLifetime.ApplicationStopping.Register(() =>
            {
                foreach (var taskExecutor in Tasks)
                {
                    taskExecutor.Value.Stop(true);
                }
            });
            initialized = true;
            Tasks.DoEach(pair => pair.Value.Start());
        }

        public bool IsInitialized { get { return initialized; } }

        public TaskSimpleExecutor AddExecutor(string key, Action action, Action<TaskExecutorBase.TaskExecutorOptions> executorOptions = null)
        {
            var taskExecutor = new TaskSimpleExecutor().SetAction(action).SetOptions(executorOptions);
            if (Tasks.TryAdd(key, taskExecutor))
                return taskExecutor as TaskSimpleExecutor;
            return null;
        }

        public TaskSequentialExecutor<TItem> AddSequentalExecutor<TItem>(string key, SequentialTaskQueryBase<TItem> query, Func<TItem, SequentialTaskCommandBase<TItem>> createCommand, Action<TaskExecutorBase.TaskExecutorOptions> executorOptions = null)
        {
            var taskExecutor = new TaskSequentialExecutor<TItem>(query, createCommand).SetOptions(executorOptions);
            if (Tasks.TryAdd(key, taskExecutor))
                return taskExecutor as TaskSequentialExecutor<TItem>;
            return null;
        }
    }
}