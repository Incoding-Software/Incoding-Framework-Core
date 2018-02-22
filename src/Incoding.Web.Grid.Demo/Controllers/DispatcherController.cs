using System;
using Incoding.Web.MvcContrib;

namespace Incoding.Web.Grid.Demo.Controllers
{
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider)
                : base(serviceProvider) { }
    }
}