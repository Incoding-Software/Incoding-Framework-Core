using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest80.Operations.Test
{
    public class DoSomeCommand : CommandBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        protected override void Execute()
        {
            var entity = Dispatcher.Push(new CreateOrCloneEntity<ItemEntity>
            {
                Id = Id,
                IsEqual = attribute => attribute.Name == Name
            });
        }
    }
}