namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Core.Block.Core;
    using Incoding.Core.Block.IoC;
    using Incoding.Core.CQRS.Core;
    using Incoding.Core.Data;
    using Incoding.Core.Extensions;
    using Incoding.Core.Extensions.LinqSpecs;
    using Machine.Specifications;
    using Moq;

    #endregion

    #region << Using >>

    #endregion

    public static class MockMessage
    {
        #region Factory constructors

        public static void Execute(this IMessage message)
        {
            if (message is MessageBase messageBase)
                message.OnExecute(IoCFactory.Instance.TryResolve<IDispatcher>(), new Lazy<IUnitOfWork>(() => IoCFactory.Instance.TryResolve<IUnitOfWork>()));
            else
                message.OnExecuteAsync(IoCFactory.Instance.TryResolve<IDispatcher>(), new Lazy<IUnitOfWork>(() => IoCFactory.Instance.TryResolve<IUnitOfWork>())).ConfigureAwait(false);
        }

        #endregion
    }

    public abstract class MockMessage<TMessage, TResult> where TMessage : IMessage
    {
        #region Constructors

        protected MockMessage(TMessage instanceMessage)
        {
            Original = instanceMessage;
            Original.Setting = new MessageExecuteSetting();
            repository = Pleasure.Mock<IRepository>();

            var unitOfWork = Pleasure.MockStrictAsObject<IUnitOfWork>(mock => mock.Setup(x => x.GetRepository()).Returns(repository.Object));
            IoCFactory.Instance.StubTryResolve(unitOfWork);

            dispatcher = Pleasure.MockStrict<IDispatcher>();
            dispatcher.Setup(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Any(message => this.predcatesStubs.Any(func => func(message))).ShouldBeTrue())));
            dispatcher.Setup(r => r.Push(Pleasure.MockIt.Is<CommandBase>(command => this.predcatesStubs.Any(func => func(command)).ShouldBeTrue())));
            IoCFactory.Instance.StubTryResolve(dispatcher.Object);
        }

        #endregion

        #region Properties

        [Obsolete("Use mock.Execute() instead of mock.Original.Execute()", false)]
        public TMessage Original { get; set; }

        #endregion

        #region Fields

        protected readonly Mock<IDispatcher> dispatcher;

        readonly Dictionary<Type, List<CommandBase>> stubs = new Dictionary<Type, List<CommandBase>>();

        readonly Dictionary<Type, List<CommandBaseAsync>> stubsAsync = new Dictionary<Type, List<CommandBaseAsync>>();

        readonly Dictionary<Type, int> stubsOfSuccess = new Dictionary<Type, int>();

        readonly Mock<IRepository> repository;

        private readonly List<Func<IMessage, bool>> predcatesStubs = new List<Func<IMessage, bool>>();

        #endregion

        #region Api Methods

        public void Execute()
        {
            Original.Execute();
            ShouldBePushed();
            repository.VerifyAll();
        }

        public MockMessage<TMessage, TResult> StubPushAsThrow<TCommand>(TCommand command, Exception ex, MessageExecuteSetting setting = null) where TCommand : CommandBase
        {
            dispatcher.StubPushAsThrow(command, ex, setting);
            return this;
        }

        public void ShouldBePushed()
        {
            foreach (var stub in stubs)
            {
                if (stubsOfSuccess.GetOrDefault(stub.Key) != stub.Value.Count)
                    throw new SpecificationException("Not Stub for {0}".F(stub.Key.Name));
            }
        }

        public MockMessage<TMessage, TResult> StubPush<TCommand>(TCommand command, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null, MessageExecuteSetting setting = null, object result = null) where TCommand : CommandBase
        {
            command.Setting = command.Setting ?? (setting ?? new MessageExecuteSetting());
            var type = typeof(TCommand);
            var value = stubs.GetOrDefault(type, new List<CommandBase>());
            value.Add(command);
            if (!stubs.ContainsKey(type))
                stubs.Add(type, value);
            predcatesStubs.Add(s =>
                               {
                                   bool isAny = false;
                                   foreach (var pair in this.stubs[type])
                                   {
                                       try
                                       {
                                           var sAsT = s as TCommand;
                                           sAsT.ShouldEqualWeak(pair as TCommand,
                                                                factoryDsl =>
                                                                {
                                                                    factoryDsl.ForwardToAction(r => r.Setting,
                                                                                               a =>
                                                                                               {
                                                                                                   if (a.Setting != null)
                                                                                                       a.Setting.ShouldEqualWeak(sAsT.Setting);
                                                                                               });
                                                                    if (dsl != null)
                                                                        dsl(factoryDsl);
                                                                });
                                           isAny = true;
                                           s.SetValue("Result", result);
                                           if (this.stubsOfSuccess.ContainsKey(type))
                                               this.stubsOfSuccess[type]++;
                                           else
                                               this.stubsOfSuccess.Add(type, 1);
                                           break;
                                       }
                                       catch (InternalSpecificationException ex)
                                       {
                                           Console.WriteLine(ex);
                                       }
                                   }

                                   return isAny;
                               });
            return this;
        }

        public MockMessage<TMessage, TResult> StubPushT<TCommand, T>(TCommand command, T result, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null, MessageExecuteSetting setting = null) where TCommand : CommandBase<T>
        {
            command.Setting = command.Setting ?? (setting ?? new MessageExecuteSetting());
            var type = typeof(TCommand);
            Action<TCommand> verify = cmd =>
            {
                bool isAny = false;
                try
                {
                    cmd.ShouldEqualWeak(command as TCommand, factoryDsl =>
                    {
                        factoryDsl.ForwardToAction(r => r.Setting, a =>
                        {
                            if (a.Setting != null)
                                a.Setting.ShouldEqualWeak(command.Setting);
                        });
                        if (dsl != null)
                            dsl(factoryDsl);
                    });
                    isAny = true;
                    command.SetValue("Result", result);
                    if (this.stubsOfSuccess.ContainsKey(type))
                        this.stubsOfSuccess[type]++;
                    else
                        this.stubsOfSuccess.Add(type, 1);
                }
                catch (InternalSpecificationException ex)
                {
                    Console.WriteLine(ex);
                }

                isAny.ShouldBeTrue();
            };
            dispatcher.Setup(x => x.Push<T>(Pleasure.MockIt.Is<TCommand>(verify))).Returns(result);

            return this;
        }

        public MockMessage<TMessage, TResult> StubPushAsync<TCommand>(TCommand command, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null, MessageExecuteSetting setting = null, object result = null) where TCommand : CommandBaseAsync
        {
            command.Setting = command.Setting ?? (setting ?? new MessageExecuteSetting());
            var type = typeof(TCommand);
            var value = stubsAsync.GetOrDefault(type, new List<CommandBaseAsync>());
            value.Add(command);
            if (!stubsAsync.ContainsKey(type))
                stubsAsync.Add(type, value);
            predcatesStubs.Add(s =>
                               {
                                   bool isAny = false;
                                   foreach (var pair in this.stubs[type])
                                   {
                                       try
                                       {
                                           var sAsT = s as TCommand;
                                           sAsT.ShouldEqualWeak(pair as TCommand,
                                                                factoryDsl =>
                                                                {
                                                                    factoryDsl.ForwardToAction(r => r.Setting,
                                                                                               a =>
                                                                                               {
                                                                                                   if (a.Setting != null)
                                                                                                       a.Setting.ShouldEqualWeak(sAsT.Setting);
                                                                                               });
                                                                    if (dsl != null)
                                                                        dsl(factoryDsl);
                                                                });
                                           isAny = true;
                                           s.SetValue("Result", result);
                                           if (this.stubsOfSuccess.ContainsKey(type))
                                               this.stubsOfSuccess[type]++;
                                           else
                                               this.stubsOfSuccess.Add(type, 1);
                                           break;
                                       }
                                       catch (InternalSpecificationException ex)
                                       {
                                           Console.WriteLine(ex);
                                       }
                                   }

                                   return isAny;
                               });
            return this;
        }

        public MockMessage<TMessage, TResult> StubPush<TCommand>(Action<IInventFactoryDsl<TCommand>> configure, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null) where TCommand : CommandBase
        {
            return StubPush(Pleasure.Generator.Invent(configure), dsl);
        }

        public MockMessage<TMessage, TResult> StubPushAsync<TCommand>(Action<IInventFactoryDsl<TCommand>> configure, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null) where TCommand : CommandBaseAsync
        {
            return StubPushAsync(Pleasure.Generator.Invent(configure), dsl);
        }

        public void ShouldBeDeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.DeleteByIds<TEntity>(Pleasure.MockIt.IsStrong(ids)));
        }

        public void ShouldBeDeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.DeleteAll<TEntity>());
        }

        public void ShouldBeDelete<TEntity>(object id, int callCount = 1) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.Delete<TEntity>(id), Times.Exactly(callCount));
        }

        public void ShouldBeDelete<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.Delete(Pleasure.MockIt.IsStrong(entity)), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                        {
                                            verify(s);
                                            return true;
                                        };

            repository.Verify(r => r.Save(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaves<TEntity>(Action<IEnumerable<TEntity>> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<IEnumerable<TEntity>, bool> match = s =>
                                                     {
                                                         verify(s);
                                                         return true;
                                                     };

            repository.Verify(r => r.Saves(Pleasure.MockIt.Is<IEnumerable<TEntity>>(entities => match(entities))), Times.Exactly(callCount));
        }

        public void ShouldBeFlush(int callCount = 1)
        {
            repository.Verify(r => r.Flush(), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            ShouldBeSave<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public void ShouldNotBeSave<TEntity>() where TEntity : class, IEntity, new()
        {
            ShouldBeSave<TEntity>(r => r.ShouldBeAssignableTo<TEntity>(), 0);
        }

        public void ShouldNotBeSaveOrUpdate<TEntity>() where TEntity : class, IEntity, new()
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldBeAssignableTo<TEntity>(), 0);
        }

        public void ShouldBeSaveOrUpdate<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                        {
                                            verify(s);
                                            return true;
                                        };

            repository.Verify(r => r.SaveOrUpdate(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaveOrUpdate<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TQuery query, TNextResult result) where TQuery : QueryBase<TNextResult>
        {
            dispatcher.StubQuery(query, result, new MessageExecuteSetting());
            dispatcher.StubQuery(query, result, null);
            return this;
        }

        public MockMessage<TMessage, TResult> StubQueryAsync<TQuery, TNextResult>(TQuery query, TNextResult result) where TQuery : QueryBaseAsync<TNextResult>
        {
            dispatcher.StubQueryAsync(query, result, new MessageExecuteSetting());
            dispatcher.StubQueryAsync(query, result, null);
            return this;
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TQuery query, Action<ICompareFactoryDsl<TQuery, TQuery>> dsl, TNextResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TNextResult>
        {
            dispatcher.StubQuery(query, dsl, result, executeSetting);
            return this;
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TNextResult result) where TQuery : QueryBase<TNextResult>
        {
            return StubQuery(Pleasure.Generator.Invent<TQuery>(), result);
        }

        public MockMessage<TMessage, TResult> StubQueryAsync<TQuery, TNextResult>(TNextResult result) where TQuery : QueryBaseAsync<TNextResult>
        {
            return StubQueryAsync(Pleasure.Generator.Invent<TQuery>(), result);
        }

        public MockMessage<TMessage, TResult> StubQueryAsNull<TQuery, TNextResult>() where TQuery : QueryBase<TNextResult>
        {
            return StubQuery<TQuery, TNextResult>(default(TNextResult));
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(Action<IInventFactoryDsl<TQuery>> configure, TNextResult result) where TQuery : QueryBase<TNextResult>
        {
            return StubQuery(Pleasure.Generator.Invent(configure), result);
        }

        public MockMessage<TMessage, TResult> StubQueryAsync<TQuery, TNextResult>(Action<IInventFactoryDsl<TQuery>> configure, TNextResult result) where TQuery : QueryBaseAsync<TNextResult>
        {
            return StubQueryAsync(Pleasure.Generator.Invent(configure), result);
        }

        #endregion

        #region Data

        public void ShouldBeIsResult(TResult expected)
        {
            Original.Result.ShouldEqualWeak(expected);
        }

        public void ShouldBeIsResult(Action<TResult> verifyResult)
        {
            verifyResult((TResult)Original.Result);
        }

        #endregion

        #region Stubs

        #region Api Methods

        public MockMessage<TMessage, TResult> StubQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false, params TEntity[] entities) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, skipInterceptions, entities));
        }

        public MockMessage<TMessage, TResult> StubEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, skipInterceptions, Pleasure.ToArray<TEntity>()));
        }

        public MockMessage<TMessage, TResult> StubPaginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, bool skipInterceptions = false, IncPaginatedResult<TEntity> result = null) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubPaginated(paginatedSpecification, orderSpecification, whereSpecification, fetchSpecification, skipInterceptions, result));
        }

        public MockMessage<TMessage, TResult> StubNotEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, bool skipInterceptions = false, int countEntity = 1) where TEntity : class, IEntity, new()
        {
            var entities = Pleasure.ToList<TEntity>();
            for (int i = 0; i < countEntity; i++)
                entities.Add(Pleasure.Generator.Invent<TEntity>());

            return StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, skipInterceptions, entities.ToArray());
        }

        public MockMessage<TMessage, TResult> StubGetById<TEntity>(object id, TEntity res) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubGetById(id, res));
        }
        public MockMessage<TMessage, TResult> StubGetByIdAsync<TEntity>(object id, TEntity res) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubGetByIdAsync(id, res));
        }

        public MockMessage<TMessage, TResult> StubSave<TEntity>(TEntity res, object id = null) where TEntity : class, IEntity, new()
        {
            Action<TEntity> verify = entity =>
                                     {
                                         entity.ShouldEqualWeak(res, null);
                                         if (id != null)
                                             entity.SetValue("Id", id);
                                     };
            return Stub(message => message.repository.Setup(r => r.Save(Pleasure.MockIt.Is<TEntity>(verify))));
        }

        public MockMessage<TMessage, TResult> StubSave<TEntity>(Action<TEntity> action, object id) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                        {
                                            action(s);
                                            return true;
                                        };

            Action<TEntity> verify = entity =>
                                     {
                                         match(entity);
                                         entity.SetValue("Id", id);
                                     };
            return Stub(message => message.repository.Setup(r => r.Save(Pleasure.MockIt.Is<TEntity>(verify))));
        }

        public MockMessage<TMessage, TResult> StubLoadById<TEntity>(object id, TEntity res) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubLoadById(id, res));
        }

        #endregion

        MockMessage<TMessage, TResult> Stub(Action<MockMessage<TMessage, TResult>> configureMock)
        {
            configureMock(this);
            return this;
        }

        #endregion
    }
}