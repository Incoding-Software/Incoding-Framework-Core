using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.Web.Extensions;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Http;
#if netcoreapp2_1
using Microsoft.AspNetCore.Http.Internal;
#endif
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

    #endregion

    /// <summary>
    ///     Wrapping MVC Controller and Mock everything inside
    /// </summary>
    /// <typeparam name="TController">MVC Controller</typeparam>
    public class MockController<TController> where TController : IncControllerBase
    {
        // mocking IDispatcher interface
        #region Fields

        readonly Mock<IDispatcher> dispatcher;

        // storing original controller to call it's functions for actual code execution
        readonly TController originalController;

        // mocking all HttpContext calls
        readonly Mock<HttpContext> httpContext;

        // mocking IView calls
        Mock<IView> view;

        // mocking IViewEngine calls
        Mock<IViewEngine> viewEngine;

        #endregion

        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="controller">original controller</param>
        /// <param name="dispatcher">mock of IDispatcher</param>
        MockController(TController controller, Mock<IDispatcher> dispatcher)
        {
            this.dispatcher = dispatcher;
            originalController = controller;

            httpContext = Pleasure.Mock<HttpContext>();

            // preparing RouteData for ControllerContext
            var routeData = new RouteData();
            routeData.Values.Add("Action", "Action");
            routeData.Values.Add("Controller", "Controller");

            var actionContext = new ActionContext(httpContext.Object, routeData, new ActionDescriptor());
            // replacing original controller Context with Mock objects
            originalController.ControllerContext = new ControllerContext(actionContext);

            // replacing original controller UrlHelper with Mock objects
            originalController.Url = new UrlHelper(actionContext);

            // replace MVC ViewEngines with Mock objects
            AddViewEngine();
        }

        #endregion

        #region Factory constructors

        /// <summary>
        ///     Static method to construct MockController object
        /// </summary>
        /// <param name="ctorArgs">original controller constructor parameters</param>
        /// <returns></returns>
        public static MockController<TController> When(params object[] ctorArgs)
        {
            var dispatcher = Pleasure.Mock<IDispatcher>();

            // Return Mock when call to IoC helper IoCFactory.Instance.TryResolve()
            //IoCFactory.Instance.StubTryResolve(dispatcher.Object);

            var controller = (TController)Activator.CreateInstance(typeof(TController), ctorArgs.ToArray());
            var res = new MockController<TController>(controller, dispatcher);
            res.httpContext.SetupGet(r => r.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "X-Requested-With", "XMLHttpRequest" } }));

            return res;
        }

        #endregion

        // return original controller
        #region Properties

        public TController Original { get { return originalController; } }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Disabling Ajax (no headers setup)
        /// </summary>
        /// <returns></returns>
        public MockController<TController> DisableAjax()
        {
            return SetupHttpContext(mock => mock.SetupGet(r => r.Request.Headers).Returns(new HeaderDictionary()));
        }

        /// <summary>
        ///     Mocking Request.Url calls
        /// </summary>
        /// <param name="requestUri">url</param>
        /// <returns></returns>
        public MockController<TController> StubRequestUrl(Uri requestUri)
        {
            return SetupHttpContext(mock =>
                                    {
                                        mock.SetupGet(r => r.Request.Host).Returns(new HostString(requestUri.Host));
                                        mock.SetupGet(r => r.Request.Path).Returns(new PathString(requestUri.AbsolutePath));
                                        mock.SetupGet(r => r.Request.QueryString).Returns(new QueryString(requestUri.Query));
                                    });
        }

        /// <summary>
        ///     Configuring HttpContextBase outside
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public MockController<TController> SetupHttpContext(Action<Mock<HttpContext>> action)
        {
            action(httpContext);
            return this;
        }

        /// <summary>
        ///     Mock Url.Action calls
        /// </summary>
        /// <param name="expectedRoute">expected route url</param>
        /// <returns></returns>
        public MockController<TController> StubUrlAction(string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                    {
                                        //mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(expectedRoute))).Returns(expectedRoute);
                                        mock.Setup(r => r.Request.PathBase).Returns("/");
                                    });
        }

        /// <summary>
        ///     Mock Url.Action calls
        /// </summary>
        /// <param name="verifyRoutes">action to verify route url</param>
        /// <param name="expectedRoute">expected route url</param>
        /// <returns></returns>
        public MockController<TController> StubUrlAction(Action<string> verifyRoutes, string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                    {
                                        //mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.Is(verifyRoutes))).Returns(expectedRoute);
                                        mock.Setup(r => r.Request.PathBase).Returns("/");
                                    });
        }

        /// <summary>
        ///     Mock calls to QueryBase derived classes and return own data
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <param name="executeSetting"></param>
        /// <returns></returns>
        public MockController<TController> StubQuery<TQuery, TResult>(TQuery query, TResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult>
        {
            dispatcher.StubQuery(query, result, executeSetting);
            return this;
        }

        /// <summary>
        ///     Mock HttpContext.User object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public MockController<TController> StubPrincipal(ClaimsPrincipal principal)
        {
            return SetupHttpContext(mock => mock.Setup(r => r.User).Returns(principal));
        }

        /// <summary>
        ///     Mock HttpContext.QueryString object
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public MockController<TController> StubQueryString(object values)
        {
            var dictionary = new Dictionary<string, StringValues>();
            foreach (var keyValuePair in AnonymousHelper.ToDictionary(values))
                dictionary.Set(keyValuePair.Key, keyValuePair.Value.ToString());

            httpContext
                    .SetupGet(r => r.Request.Query)
                    .Returns(new QueryCollection(dictionary));

            return this;
        }

        /// <summary>
        ///     Throw an exception for Dispatcher.Push call
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="exception"></param>
        /// <param name="executeSetting"></param>
        /// <returns></returns>
        public MockController<TController> StubPushAsThrow<TCommand>(TCommand command, Exception exception, MessageExecuteSetting executeSetting = null) where TCommand : CommandBase
        {
            dispatcher.StubPushAsThrow(command, exception, executeSetting);
            return this;
        }

        /// <summary>
        ///     Throw an exception for Dispatcher.Query call
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public MockController<TController> StubQueryAsThrow<TQuery, TResult>(TQuery query, Exception exception) where TQuery : QueryBase<TResult>
        {
            dispatcher.StubQueryAsThrow<TQuery, TResult>(query, exception);
            return this;
        }

        /// <summary>
        ///     Assert View is rendered
        /// </summary>
        /// <param name="viewName"></param>
        public void ShouldBeRenderView(string viewName = "Action")
        {
            ShouldBeRenderModel(null, viewName);
        }

        /// <summary>
        ///     Assert Model on View is rendered
        /// </summary>
        /// <param name="model"></param>
        /// <param name="viewName"></param>
        public void ShouldBeRenderModel(object model, string viewName = "Action")
        {
            ShouldBeRenderModel<object>(o => o.ShouldEqualWeak(model), viewName);
        }

        /// <summary>
        ///     Assert Model on View is rendered with action verify
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="action"></param>
        /// <param name="viewName"></param>
        public void ShouldBeRenderModel<TModel>(Action<TModel> action, string viewName = "Action")
        {
            Action<ViewContext> verify = s => action((TModel)s.ViewData.Model);
            view.Verify(r => r.RenderAsync(Pleasure.MockIt.Is(verify)));
            viewEngine.Verify(r => r.FindView(Pleasure.MockIt.IsAny<ControllerContext>(), viewName, Pleasure.MockIt.IsAny<bool>()));
        }

        /// <summary>
        ///     Mock Dispatcher.Push call
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="executeSetting"></param>
        /// <param name="callCount"></param>
        public void ShouldBePush<TCommand>(TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            dispatcher.ShouldBePush(command, executeSetting, callCount);
        }

        /// <summary>
        ///     Assert Dispatcher.Push wasn't called
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        public void ShouldNotBePush<TCommand>(TCommand command) where TCommand : CommandBase
        {
            ShouldBePush(command, callCount: 0);
        }

        /// <summary>
        ///     Assert Dispatcher.Push call with action verifier
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="action"></param>
        /// <param name="callCount"></param>
        public void ShouldBePush<TCommand>(Action<TCommand> action, int callCount = 1) where TCommand : CommandBase
        {
            dispatcher.ShouldBePush(action, callCount);
        }

        /// <summary>
        ///     Generate invalid ModelState with ModelError
        /// </summary>
        /// <returns></returns>
        public MockController<TController> BrokenModelState()
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(Pleasure.Generator.String(), Pleasure.Generator.String());
            originalController.ViewData.SetValue("_modelState", modelStateDictionary);
            return this;
        }

        #endregion

        // Mock MVC ViewEngines
        void AddViewEngine()
        {
            view = Pleasure.Mock<IView>();
            viewEngine = Pleasure.Mock<IViewEngine>();
            viewEngine.Setup(r => r.FindView(Pleasure.MockIt.IsAny<ControllerContext>(), Pleasure.MockIt.IsAny<string>(), Pleasure.MockIt.IsAny<bool>())).Returns(ViewEngineResult.Found(view.Object.Path, view.Object));
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(viewEngine.Object);
        }
    }
}