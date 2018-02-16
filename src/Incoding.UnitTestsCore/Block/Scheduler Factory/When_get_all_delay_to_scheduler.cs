using Incoding.Core.Block.Core;
using Incoding.Core.Block.Scheduler;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetSchedulersQuery))]
    public class When_get_all_delay_to_scheduler
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<GetSchedulersQuery>();
                                  fakeCommand = Pleasure.Generator.Invent<FakeCommand>();
                                  Action<IInventFactoryDsl<DelayToScheduler>> action = dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand).AssemblyQualifiedName)
                                                                                                 .GenerateTo(r => r.Option)
                                                                                                 .Tuning(r => r.Command, fakeCommand.ToJsonString());
                                  successEntities = new[]
                                                    {
                                                            Pleasure.Generator.Invent(action),
                                                            Pleasure.Generator.Invent(action),
                                                    };

                                  mockQuery = MockQuery<GetSchedulersQuery, List<GetSchedulersQuery.Response>>
                                          .When(query)
                                          .StubQuery(whereSpecification: new DelayToScheduler.Where.ByStatus(new[] { DelayOfStatus.New, DelayOfStatus.Error, DelayOfStatus.InProgress })
                                                             .And(new DelayToScheduler.Where.ByAsync(query.Async))
                                                             .And(new DelayToScheduler.Where.AvailableStartsOn(query.Date)),
                                                     orderSpecification: new DelayToScheduler.Sort.Default(),
                                                     paginatedSpecification: new PaginatedSpecification(1, query.FetchSize),
                                                     entities: successEntities);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(successEntities, (dsl, i) => dsl.ForwardToValue(r => r.TimeOut, successEntities[i].Option.TimeOut)
                                                                                                                                .ForwardToValue(r => r.Instance, fakeCommand)));

        #region Fake classes

        public class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<GetSchedulersQuery, List<GetSchedulersQuery.Response>> mockQuery;

        static DelayToScheduler[] successEntities;

        static FakeCommand fakeCommand;

        #endregion
    }
}