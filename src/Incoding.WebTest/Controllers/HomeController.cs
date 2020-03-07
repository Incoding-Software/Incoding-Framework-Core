using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc;
using Incoding.WebTest.Models;
using Incoding.WebTest.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Incoding.WebTest.Controllers
{
    public class HomeController : IncControllerBase
    {
        public IActionResult Index(GetItemsQuery query)
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddItemFile(AddItemCommand cmd)
        {
            int a = 5;
            int b = a + 5;
            return TryPush(cmd);
        }

        public HomeController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
