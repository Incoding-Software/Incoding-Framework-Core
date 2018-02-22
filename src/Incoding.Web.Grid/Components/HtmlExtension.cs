using Incoding.Web.Grid.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.Grid.Components
{
    public static class HtmlExtension
    {
        public static IGridBuilder<TModel> Grid<TModel>(this IHtmlHelper htmlHelper) where TModel : class
        {
            return new GridBuilder<TModel>(htmlHelper);
        }
    }
}