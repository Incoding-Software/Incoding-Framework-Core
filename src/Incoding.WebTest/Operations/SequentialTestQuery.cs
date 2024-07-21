using System.Collections.Generic;
using System.Threading.Tasks;
using Incoding.Core.Tasks;

namespace Incoding.WebTest.Operations
{
    public class SequentialTestQuery : SequentialTaskQueryBase<SequentialTestQuery.Response>
    {
        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        protected override async Task<IEnumerable<Response>> ExecuteResult()
        {
            return new List<Response>
            {
                new Response() { Id = 1, Name = "The One"},
                new Response() { Id = 2, Name = "The Two"}
            };
        }
    }
}