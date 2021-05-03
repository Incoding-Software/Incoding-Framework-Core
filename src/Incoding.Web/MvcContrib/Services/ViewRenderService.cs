using System;
using System.IO;
using System.Threading.Tasks;
using Incoding.Core.Block.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(Controller controller, string viewName, object model);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToStringAsync(Controller controller, string viewName, object model)
        {
            using (var sw = new StringWriter())
            {

                var viewResult = _viewEngine.GetView(viewName, viewName, false);
                if(!viewResult.Success)
                    viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);

                //if (!viewResult.Success)
                //{
                //    var endPointDisplay = controller.HttpContext.Request.GetEndpoint().DisplayName;

                //    if (endPointDisplay.Contains(".Areas."))
                //    {
                //        //search in Areas
                //        var areaName = endPointDisplay.Substring(endPointDisplay.IndexOf(".Areas.") + ".Areas.".Length);
                //        areaName = areaName.Substring(0, areaName.IndexOf(".Controllers."));

                //        var viewNamePath = $"~/Areas/{areaName}/views/{controller.HttpContext.Request.RouteValues["controller"]}/{controller.HttpContext.Request.RouteValues["action"]}.cshtml";

                //        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                //    }

                //    if (!viewResult.Success)
                //        throw new Exception($"A view with the name '{viewNamePath}' could not be found");
                //}

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                //var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                //{
                //    Model = model
                //};

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}