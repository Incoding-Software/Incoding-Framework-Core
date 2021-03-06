﻿
namespace Incoding.Web.Grid.Interfaces
{
    public interface IPageableBuilder<T> where T : class
    {
        IPageableBuilder<T> ItemsCount(bool showItemsCount);

        IPageableBuilder<T> PageSizes(bool showPageSizes);

        IPageableBuilder<T> PageSizes(params int[] pageSizesArray);

        // IPageableBuilder<T> ButtonCount(int count);

    }
}