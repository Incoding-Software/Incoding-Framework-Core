using System;
using Incoding.Web.MvcContrib;

namespace Incoding.WebTest.Controllers
{
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }
    }
}