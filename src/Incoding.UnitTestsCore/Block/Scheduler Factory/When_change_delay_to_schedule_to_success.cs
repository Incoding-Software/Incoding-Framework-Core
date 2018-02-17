using Incoding.Core.Block.Scheduler.Command;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpec;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ChangeSchedulerStatusCommand))]
    public class When_change_delay_to_schedule_to_success
    {
        Establish establish = () =>
                              {
                                  var command = Pleasure.Generator.Invent<ChangeSchedulerStatusCommand>(dsl => dsl.Tuning(s => s.Status, DelayOfStatus.Success));

                                  var instance = Pleasure.Generator.Invent<ChangeSchedulerStatusCommand>();
                                  DateTime? nextDt = Pleasure.Generator.DateTime();
                                  lastStartOn = Pleasure.Generator.DateTime();
                                  delay = Pleasure.MockStrict<DelayToScheduler>(mock =>
                                                                                {
                                                                                    mock.SetupGet(r => r.StartsOn).Returns(lastStartOn);
                                                                                    mock.SetupGet(r => r.Priority).Returns(Pleasure.Generator.PositiveNumber());
                                                                                    mock.SetupSet(r => r.Status = command.Status);
                                                                                    mock.SetupSet(r => r.Description = command.Description);
                                                                                    mock.SetupGet(r => r.Recurrence).Returns(recurrency);
                                                                                    mock.SetupGet(r => r.UID).Returns(Guid.NewGuid().ToString);
                                                                                });

                                  recurrency = Pleasure.Generator.Invent<GetRecurrencyDateQuery>(dsl => dsl.Tuning(s => s.NowDate, lastStartOn));

                                  Action<ICompareFactoryDsl<ScheduleCommand, ScheduleCommand>> compare
                                          = dsl => dsl.ForwardToAction(r => r.Recurrency, schedulerCommand => schedulerCommand.Recurrency.ShouldEqualWeak(recurrency,
                                                                                                                                                          factoryDsl => factoryDsl.ForwardToValue(r => r.NowDate, null)
                                                                                                                                                                                  .ForwardToValue(r => r.RepeatCount, recurrency.RepeatCount - 1)
                                                                                                                                                                                  .ForwardToValue(r => r.StartDate, lastStartOn)));
                                  mockCommand = MockCommand<ChangeSchedulerStatusCommand>
                                          .When(command)
                                          .StubGetById(command.Id, delay.Object)
                                          .StubQuery(recurrency, nextDt)
                                          .StubPush(dsl => dsl.Tuning(r => r.UID, delay.Object.UID)
                                                              .Tuning(r => r.Command, instance)                                                              
                                                              .Tuning(r => r.Priority, delay.Object.Priority), compare);
                              };

        Because of = () => mockCommand.Execute();

        It should_be_update_start_on = () => recurrency.NowDate.ShouldEqual(lastStartOn);

        It should_be_verify = () => delay.VerifyAll();

        #region Establish value

        static MockMessage<ChangeSchedulerStatusCommand, object> mockCommand;

        static Mock<DelayToScheduler> delay;

        static DateTime lastStartOn;

        static GetRecurrencyDateQuery recurrency;

        #endregion
    }
}