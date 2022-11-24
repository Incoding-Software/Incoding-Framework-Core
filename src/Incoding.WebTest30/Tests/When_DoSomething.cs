using Incoding.UnitTests.MSpec;
using Incoding.WebTest30.Operations;
using Incoding.WebTest30.Operations.Test;
using Machine.Specifications;

namespace Incoding.WebTest30.Tests
{
    public class When_DoSomething
    {
        private It when1 = () =>
        {
            var command = Pleasure.Generator.Invent<DoSomeCommand>();
            var entity = Pleasure.Generator.Invent<ItemEntity>();

            var mockCommand = MockCommand<DoSomeCommand>.When(command)
                .StubPushT(new CreateOrCloneEntity<ItemEntity>() {Id = command.Id},
                    entity,
                    dsl => dsl.ForwardToAction(r => r.IsEqual, cloneEntity => cloneEntity.IsEqual.Invoke(Pleasure.Generator.Invent<ItemEntity>()))
                        );

            mockCommand.Execute();
        };
    }
}