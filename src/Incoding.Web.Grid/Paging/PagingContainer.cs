using System.Collections.Generic;

namespace Incoding.Web.Grid.Paging
{
    public class PagingContainer
    {
        public string Start { get; set; }

        public string End { get; set; }

        public string Total { get; set; }

        public bool IsFirst  { get; set; }

        public bool HasNext { get; set; }
        
        public bool IsLast  { get; set; }

        public List<PagingModel> Items { get; set; }
    }
}