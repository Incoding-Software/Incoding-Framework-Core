using System;
using System.Diagnostics.CodeAnalysis;
using Incoding.Block.IoC;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Incoding.Mvc.MvcContrib.Core
{
    #region << Using >>

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly, Obsolete("On next version should be removed. Please do not use IoC on ctor controller")]
    public class IncControllerFactory : DefaultControllerFactory
    {
        #region Fields

        readonly string[] rootNamespaces = new string[] { };

        #endregion

        #region Override

        public override object CreateController(ControllerContext context)
        {
            return base.CreateController(context);
        }

        ////ncrunch: no coverage start
        protected override Controller GetControllerInstance(RequestContext requestContext, Type controllerType)
        {            
            if (controllerType == null)
            {
                var originalNamespace = requestContext.RouteData.DataTokens["Namespaces"];
                var originalArea = requestContext.RouteData.DataTokens["area"];

                requestContext.RouteData.DataTokens["Namespaces"] = this.rootNamespaces;
                requestContext.RouteData.DataTokens["area"] = "";
                controllerType = GetControllerType(requestContext, requestContext.RouteData.Values["controller"].ToString());
                requestContext.RouteData.DataTokens["Namespaces"] = originalNamespace;
                requestContext.RouteData.DataTokens["area"] = originalArea;
            }

            return IoCFactory.Instance.Resolve<IController>(controllerType);
        }

        ////ncrunch: no coverage end

        #endregion

        #region Constructors

        public IncControllerFactory() { }

        public IncControllerFactory(string[] rootNamespaces)
        {
            this.rootNamespaces = rootNamespaces;
        }

        #endregion
    }
}