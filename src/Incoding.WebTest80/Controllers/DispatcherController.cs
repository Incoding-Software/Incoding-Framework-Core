using Incoding.Web.MvcContrib;

namespace Incoding.WebTest80.Controllers
{
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }
    }
}