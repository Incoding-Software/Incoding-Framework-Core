﻿using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Core;
using Incoding.Mvc.MvcContrib.MVD.Core;

namespace Incoding.Mvc.MvcContrib.MVD.Handlers
{
    #region << Using >>

    #endregion

    public class PushHttpHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            var dispatcher = new DefaultDispatcher();
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = context.Request.Params
                                             });

            var commands = dispatcher.Query(new CreateByTypeQuery.AsCommands()
                                            {
                                                    IncTypes = parameter.Type,
                                                    IsComposite = parameter.IsCompositeArray
                                            });
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            dispatcher.Push(new CommandComposite(commands));
            var data = commands.Length == 1 ? commands[0].Result : commands.Select(r => r.Result);
            context.Response.Write(IncodingResult.Success(data).ToJsonString());
        }

        public bool IsReusable { get { return false; } }

        public static readonly IRouteHandler Route = new PushRoute();

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public class PushRoute : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new PushHttpHandler();
            }
        }

        #endregion
    }
}