using System;
using Incoding.CQRS;
using Incoding.Mvc.MvcContrib.MVD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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