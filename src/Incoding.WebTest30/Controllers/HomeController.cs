using System;
using System.Diagnostics;
using Incoding.Web.MvcContrib;
using Incoding.WebTest30.Models;
using Incoding.WebTest30.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Incoding.WebTest30.Controllers
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
