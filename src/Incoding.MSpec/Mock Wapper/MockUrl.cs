using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using Moq;

    #endregion

    public class MockUrl
    {
        #region Fields

        readonly Mock<HttpContext> httpContext;

        #endregion

        #region Constructors

        public MockUrl()
        {
            var routeData = new RouteData();
            routeData.Values.Add("Action", "Action");
            routeData.Values.Add("Controller", "Controller");
            this.httpContext = Pleasure.Mock<HttpContext>(mock =>
                                                                  {
                                                                      var httpRequestBase = Pleasure.MockAsObject<HttpRequest>(mock1 => mock1.SetupGet(r => r.PathBase).Returns("/"));
                                                                      mock.SetupGet(r => r.Request).Returns(httpRequestBase);
                                                                  });
            var actionContext = new ActionContext(this.httpContext.Object, routeData, new ActionDescriptor());
            
            Original = new UrlHelper(actionContext);
        }

        #endregion

        #region Properties

        public IUrlHelper Original { get; private set; }

        #endregion

        #region Api Methods

        public MockUrl StubAction(string url)
        {
            //TODO: mock
            //this.httpContext
            //    .SetupGet(r => r.Response)
            //    .Returns(Pleasure.MockAsObject<HttpResponse>(mock1 => mock1.Setup(r => r.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(url))).Returns(url)));
            return this;
        }

        public MockUrl StubRequest(Action<Mock<HttpRequest>> action)
        {
            var request = Pleasure.Mock<HttpRequest>();
            action(request);

            this.httpContext.SetupGet(r => r.Request).Returns(request.Object);
            return this;
        }

        #endregion
    }
}