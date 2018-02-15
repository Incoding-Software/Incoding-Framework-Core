using System;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Core;
using Incoding.Mvc.MvcContrib.Extensions;
using Incoding.Mvc.MvcContrib.MVD.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Mvc.MvcContrib.MVD
{
    #region << Using >>

    #endregion

    // ReSharper disable Mvc.ViewNotResolved
    // ReSharper disable MemberCanBeProtected.Global
    public class DispatcherControllerBase : IncControllerBase
    {
        #region Api Methods

        public DispatcherControllerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public virtual async Task<ActionResult> Validate()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
            {
                Params = HttpContext.Request.GetNameValueCollection()
            });
            // ReSharper disable once UnusedVariable
            var instance = await new CreateMessageByType()
            {
                Provider = _serviceProvider,
                Type = parameter.Type,
                ControllerContext = this.ControllerContext,
                ModelState = ModelState
            }.Execute();
            return ModelState.IsValid ? IncodingResult.Success() : IncodingResult.Error(ModelState);
        }

        public virtual async Task<ActionResult> Query()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
            {
                Params = HttpContext.Request.GetNameValueCollection()
            });
            var query = await new CreateMessageByType() { Provider = _serviceProvider, Type = parameter.Type, ControllerContext = this.ControllerContext, ModelState = ModelState }.Execute();

            if (parameter.IsValidate && !ModelState.IsValid)
                return IncodingResult.Error(ModelState);

            var composite = new CommandComposite((IMessage)query);
            return TryPush(commandComposite => dispatcher.Query(new MVDExecute(HttpContext) { Instance = composite }), composite, setting => setting.SuccessResult = () => IncodingResult.Success(composite.Parts[0].Result), isAjax: true);
        }

        public virtual async Task<ActionResult> Render()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
            {
                Params = HttpContext.Request.GetNameValueCollection()
            });
            object model = null;
            if (!string.IsNullOrWhiteSpace(parameter.Type))
            {
                var instance = await new CreateMessageByType()
                {
                    Provider = _serviceProvider,
                    Type = parameter.Type,
                    ControllerContext = ControllerContext,
                    ModelState = ModelState,
                    IsModel = parameter.IsModel
                }.Execute();

                if (parameter.IsValidate && !ModelState.IsValid)
                    return IncodingResult.Error(ModelState);

                model = parameter.IsModel ? instance : dispatcher.Query(new MVDExecute(HttpContext) { Instance = new CommandComposite((IMessage)instance) });
            }

            ModelState.Clear();

            var isAjaxRequest = HttpContext.Request.IsAjaxRequest();
            return isAjaxRequest
                           ? await IncPartialView(parameter.View, model)
                           : (ActionResult)Content(await RenderToString(parameter.View, model));
        }

        public virtual async Task<ActionResult> Push()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
            {
                Params = HttpContext.Request.GetNameValueCollection()
            });

            var commands = await new CreateMessageByType.AsCommands
            {
                Provider = _serviceProvider,
                IncTypes = parameter.Type,
                ModelState = ModelState,
                ControllerContext = ControllerContext,
                IsComposite = parameter.IsCompositeArray
            }.Execute();

            var composite = new CommandComposite(commands);
            return TryPush(commandComposite => dispatcher.Query(new MVDExecute(HttpContext) { Instance = composite }), composite, setting => setting.SuccessResult = () =>
                                                                                                                                                                     {
                                                                                                                                                                         var data = commands.Length == 1 ? commands[0].Result : commands.Select(r => r.Result);
                                                                                                                                                                         return IncodingResult.Success(data);
                                                                                                                                                                     });
        }

        public virtual async Task<ActionResult> QueryToFile()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
            {
                Params = HttpContext.Request.GetNameValueCollection()
            });
            var instance = await new CreateMessageByType()
            {
                Type = parameter.Type,
                ControllerContext = ControllerContext,
                ModelState = ModelState
            }.Execute();
            var result = dispatcher.Query(new MVDExecute(HttpContext)
            {
                Instance = new CommandComposite((IMessage)instance)
            });
            Guard.NotNull("result", result, "Result from query {0} is null but argument 'result' should be not null".F(parameter.Type));

            Response.Headers.Add("X-Download-Options", "Open");
            return File((byte[])result, parameter.ContentType, parameter.FileDownloadName);
        }

        #endregion
    }
}