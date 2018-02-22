
namespace Incoding.Web.Grid.Interfaces
{
    public interface IGridBuilder<T> where T : class
    {
        IGridBuilderOptions<T> Id(string gridId);
    }
}