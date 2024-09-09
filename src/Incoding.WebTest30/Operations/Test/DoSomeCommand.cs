using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest30.Operations.Test
{
    public class DoSomeCommand : CommandBaseAsync
    {
        public int Id { get; set; }
        public string Name { get; set; }
        protected override async Task ExecuteAsync()
        {
            var entity = await Dispatcher.PushAsync(new CreateOrCloneEntity<ItemEntity>
            {
                Id = Id,
                IsEqual = attribute => attribute.Name == Name
            });
        }

    }
}