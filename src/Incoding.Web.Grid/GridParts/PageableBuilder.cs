using Incoding.Web.Grid.Interfaces;

namespace Incoding.Web.Grid.GridParts
{
    public class PageableBuilder<T> : IPageableBuilder<T> where T : class
    {
        #region Fields

        readonly GridBuilder<T> _gridBuilder;

        #endregion

        #region Constructors

        public PageableBuilder(GridBuilder<T> gridBuilder)
        {
            this._gridBuilder = gridBuilder;
        }

        #endregion

        public IPageableBuilder<T> ItemsCount(bool showItemsCount)
        {
            _gridBuilder.SetItemsCount(showItemsCount);
            return this;
        }

        public IPageableBuilder<T> PageSizes(bool showPageSizes)
        {
            _gridBuilder.SetPageSizes(showPageSizes);
            return this;
        }

        public IPageableBuilder<T> PageSizes(params int[] pageSizesArray)
        {
            _gridBuilder.SetPageSizesArray(pageSizesArray);
            return PageSizes(true);
        }
        
    }
}