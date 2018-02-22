using GridUI.Persistance;
using GridUI.Specifications;
using Incoding.Core.Block.Core;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Incoding.Web.Grid.Interfaces;
using Incoding.Web.Grid.Paging;

namespace GridUI.Queries
{
    public class GetProductsPagingQuery : QueryBase<IncPaginatedResult<Product>>, IRoutableQuery, ISortable<GetProductsPagingQuery.SortType>
    {
        public enum SortType
        {
            Name,
            Price
        }

        protected override IncPaginatedResult<Product> ExecuteResult()
        {
            return this.Repository.Paginated(paginatedSpecification: new PaginatedSpecification(this.Page, PageSize), orderSpecification: new ProductPagingOrderSpec(SortBy, Desc));
        }

        [HashUrl]
        public int Page { get; set; }

        [HashUrl]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public string BuildUrl(int page = 0)
        {
            this.Page = page;
            return this.HashAction();
        }

        public SortType SortBy { get; set; }
        public bool Desc { get; set; }

        private int pageSize = 10;
    }
}