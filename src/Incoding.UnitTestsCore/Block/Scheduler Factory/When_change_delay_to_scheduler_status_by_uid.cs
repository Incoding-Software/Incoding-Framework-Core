using Incoding.Core.Block.Scheduler.Command;
using Incoding.Core.Block.Scheduler.Persistence;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ChangeSchedulerStatusByUIDCommand))]
    public class When_change_delay_to_scheduler_status_by_uid
    {
        #region Establish value

        static MockMessage<ChangeSchedulerStatusByUIDCommand, object> mockCommand;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  var command = Pleasure.Generator.Invent<ChangeSchedulerStatusByUIDCommand>();

                                  var entities = new[]
                                                 {
                                                         Pleasure.Generator.Invent<DelayToScheduler>(), Pleasure.Generator.Invent<DelayToScheduler>(), 
                                                 };
                                  mockCommand = MockCommand<ChangeSchedulerStatusByUIDCommand>
                                          .When(command)
                                          .StubQuery(whereSpecification: new DelayToScheduler.Where.ByUID(command.UID), 
                                                     entities: entities)
                                          .StubPush(new ChangeSchedulerStatusCommand() { Id = entities[0].Id, Status = command.Status })
                                          .StubPush(new ChangeSchedulerStatusCommand() { Id = entities[1].Id, Status = command.Status });
                              };

        Because of = () => { exception = Catch.Exception(() => mockCommand.Execute()); };

        It should_be_success = () => exception.ShouldBeNull();
    }
}