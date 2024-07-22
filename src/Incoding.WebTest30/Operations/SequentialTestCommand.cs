using System.Diagnostics;
using System.Threading.Tasks;
using Incoding.Core.Tasks;

namespace Incoding.WebTest30.Operations
{
    public class SequentialTestCommand : SequentialTaskCommandBase<SequentialTestQuery.Response>
    {
        protected override async Task ExecuteAsync()
        {
            Debug.WriteLine($"{Item.Id} : {Item.Name}");
        }
    }
}