using System.Diagnostics;
using Incoding.Core.Tasks;

namespace Incoding.WebTest80.Operations
{
    public class SequentialTestCommand : SequentialTaskCommandBase<SequentialTestQuery.Response>
    {
        protected override void Execute()
        {
            Debug.WriteLine($"{Item.Id} : {Item.Name}");
        }
    }
}