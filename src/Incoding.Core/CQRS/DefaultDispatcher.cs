using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Common;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Incoding.Core;

namespace Incoding.Core.CQRS
{
    #region << Using >>

    #endregion

    public class DefaultDispatcher : IDispatcher
    {
        static readonly List<Func<IMessageInterception>> interceptions = new List<Func<IMessageInterception>>();

        public static void SetInterception(Func<IMessageInterception> create)
        {
            interceptions.Add(create);
        }
        
        #region Fields

        readonly UnitOfWorkCollection unitOfWorkCollection = new UnitOfWorkCollection();
        
        #endregion
        
        #region Nested classes

        internal class UnitOfWorkCollection : Dictionary<MessageExecuteSetting, Lazy<IUnitOfWork>>, IDisposable
        {
            #region Disposable

            public void Dispose()
            {
                this.Select(r => r.Value)
                    .DoEach(r =>
                            {
                                if (r.IsValueCreated)
                                {
                                    r.Value.Dispose();
                                }
                            });
                Clear();
            }

            #endregion

            #region Api Methods

            public Lazy<IUnitOfWork> AddOrGet(MessageExecuteSetting setting, bool isFlush)
            {
                if (!ContainsKey(setting))
                {
                    Add(setting, new Lazy<IUnitOfWork>(() =>
                                                       {
                                                           var unitOfWorkFactory = string.IsNullOrWhiteSpace(setting.DataBaseInstance)
                                                                                           ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                                                                           : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(setting.DataBaseInstance);

                                                           var isoLevel = setting.IsolationLevel.GetValueOrDefault(isFlush ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted);
                                                           return unitOfWorkFactory.Create(isoLevel, isFlush, setting.Connection);
                                                       }, LazyThreadSafetyMode.None));
                }

                return this[setting];
            }

            #endregion

            public void Commit()
            {
                this.Select(r => r.Value)
                    .DoEach(r =>
                            {
                                if (r.IsValueCreated)
                                    r.Value.Commit();
                            });
            }
            public async Task CommitAsync()
            {
                foreach (var r in this.Select(r => r.Value))
                {
                    if (r.IsValueCreated)
                        await r.Value.CommitAsync();
                }
            }
        }

        #endregion

        #region IDispatcher Members

        public IDispatcher New()
        {
            return new DefaultDispatcher();
        }
        
        public void Push(CommandComposite composite)
        {
            bool isOuterCycle = !unitOfWorkCollection.Any();
            var isFlush = composite.Parts.Any(s => s is CommandBase || s is CommandBaseAsync);
            try
            {
                foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
                {
                    foreach (var part in groupMessage)
                    {
                        if (isOuterCycle)
                        {
                            if (part.Setting.UID == Guid.Empty)
                                part.Setting.UID = Guid.NewGuid();
                            part.Setting.IsOuter = true;
                        }
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        foreach (var interception in interceptions)
                            interception().OnBeforeAsync(part).GetAwaiter().GetResult();

                        part.OnExecute(this, unitOfWork);

                        foreach (var interception in interceptions)
                            interception().OnAfterAsync(part).GetAwaiter().GetResult();

                        var isFlushInIteration = part is CommandBase || part is CommandBaseAsync;
                        if (unitOfWork.IsValueCreated && isFlushInIteration)
                            unitOfWork.Value.Flush();
                    }
                }
                if (isOuterCycle && isFlush)
                    this.unitOfWorkCollection.Commit();
            }
            finally
            {
                if (isOuterCycle)
                    unitOfWorkCollection.Dispose();
            }
        }

        public void Push(CommandBase command)
        {
            Push(new CommandComposite(command));
        }

        public T Push<T>(CommandBase<T> command)
        {
            Push(new CommandComposite(command));
            return command.Result;
        }

        public async Task PushAsync(CommandBaseAsync command)
        {
            await PushAsyncInternal(new CommandComposite(command, new MessageExecuteSetting()));
        }
        public async Task<T> PushAsync<T>(CommandBaseAsync<T> message)
        {
            await PushAsyncInternal(new CommandComposite(message));
            return message.Result;
        }

        public async Task PushAsyncInternal(CommandComposite composite)
        {
            bool isOuterCycle = !unitOfWorkCollection.Any();
            var isFlush = composite.Parts.Any(s => s is CommandBase || s is CommandBaseAsync);
            try
            {
                foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
                {
                    foreach (var part in groupMessage)
                    {
                        if (isOuterCycle)
                        {
                            if(part.Setting.UID == Guid.Empty)
                                part.Setting.UID = Guid.NewGuid();
                            part.Setting.IsOuter = true;
                        }
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        foreach (var interception in interceptions)
                            await interception().OnBeforeAsync(part);

                        await part.OnExecuteAsync(this, unitOfWork);

                        foreach (var interception in interceptions)
                            await interception().OnAfterAsync(part);

                        var isFlushInIteration = part is CommandBase || part is CommandBaseAsync;
                        if (unitOfWork.IsValueCreated && isFlushInIteration)
                            await unitOfWork.Value.FlushAsync();
                    }
                }
                if (isOuterCycle && isFlush)
                    await this.unitOfWorkCollection.CommitAsync();
            }
            finally
            {
                if (isOuterCycle)
                    unitOfWorkCollection.Dispose();
            }
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            Push(new CommandComposite(message, executeSetting));
            return (TResult)message.Result;
        }

        public async Task<TResult> QueryAsync<TResult>(QueryBaseAsync<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            await PushAsyncInternal(new CommandComposite(message, executeSetting));
            return (TResult)message.Result;
        }

        #endregion
    }
}