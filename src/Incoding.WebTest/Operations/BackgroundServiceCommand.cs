using System.Diagnostics;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest.Operations
{
    public class BackgroundServiceCommand : CommandBaseAsync
    {
        protected override async Task ExecuteAsync()
        {
            var task = new Task(() =>
            {
                Debug.WriteLine("123");
            });
            task.Start();
            await task;
        }
    }
}