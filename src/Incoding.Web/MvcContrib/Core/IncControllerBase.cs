using System;
using System.Threading.Tasks;
using Incoding.Core;
using Incoding.Core.CQRS.Core;
using Incoding.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    // ReSharper disable PublicConstructorInAbstractClass
    public abstract class IncControllerBase : Controller
    {
        #region Fields

        protected IncControllerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }

        protected IDispatcher dispatcher;
        protected IServiceProvider _serviceProvider;

        #endregion
        
        protected IncodingResult IncRedirect(string url)
        {
            return IncodingResult.RedirectTo(url);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action)
        {
            return IncRedirectToAction(action, null);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action, [AspMvcController] string controller)
        {
            return IncRedirectToAction(action, controller, null);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action, [AspMvcController] string controller, object routes)
        {
            return IncodingResult.RedirectTo(Url.Action(action, controller, routes));
        }

        [AspMvcView]
        protected async Task<IncodingResult> IncView(object model = null)
        {
            return await IncPartialView(ControllerContext.RouteData.Values["action"].ToString(), model);
        }

        protected IncodingResult IncJson(object model)
        {
            return IncodingResult.Success(model);
        }

        protected async Task<IncodingResult> IncPartialView([AspMvcView] string viewName, object model = null)
        {
            return IncodingResult.Success(await RenderToString(viewName, model));
        }

        protected async Task<string> RenderToString([AspMvcView] string viewName, object model)
        {
            Guard.NotNullOrWhiteSpace("viewName", viewName);
            ViewData.Model = model;

            var viewResult = await _serviceProvider.GetService<IViewRenderService>().RenderToStringAsync(ControllerContext, viewName, model);
            return viewResult;
        }

        protected ActionResult TryPush(CommandBase input, Action<IncTryPushSetting> action = null)
        {
            return TryPush((Action<CommandComposite>) (composite => composite.Quote(input)), action);
        }


        protected ActionResult TryPush(Action<CommandComposite> configuration, Action<IncTryPushSetting> action = null)
        {
            var composite = new CommandComposite();
            configuration(composite);
            return TryPush(composite, action);
        }

        protected ActionResult TryPush(CommandComposite composite, Action<IncTryPushSetting> action = null)
        {
            return TryPush(commandComposite => dispatcher.Push(commandComposite), composite, action);
        }

        protected async Task<ActionResult> TryPushAsync(Func<CommandComposite, Task> push, CommandComposite composite, Action<IncTryPushSetting> action = null, bool? isAjax = null)
        {
            var setting = new IncTryPushSetting();
            action.Do(r => r(setting));

            Func<ActionResult> defaultSuccess = () => View(composite.Parts[0]);
            var isActualAjax = isAjax.GetValueOrDefault(HttpContext.Request.IsAjaxRequest());
            if (isActualAjax)
                defaultSuccess = () => IncodingResult.Success();
            var success = setting.SuccessResult ?? defaultSuccess;

            Func<IncWebException, ActionResult> defaultError = (ex) => View(composite.Parts[0]);
            if (isActualAjax)
                defaultError = (ex) => IncodingResult.Error((ModelStateDictionary)ModelState);
            var error = setting.ErrorResult ?? defaultError;

            if (!ModelState.IsValid)
                return error(IncWebException.For(string.Empty, string.Empty));

            try
            {
                await push(composite);
                return success();
            }
            catch (IncWebException exception)
            {
                foreach (var pairError in exception.Errors)
                {
                    foreach (var errorMessage in pairError.Value)
                        ModelState.AddModelError(pairError.Key, errorMessage);
                }

                return error(exception);
            }
        }

        protected ActionResult TryPush(Action<CommandComposite> push, CommandComposite composite, Action<IncTryPushSetting> action = null, bool? isAjax = null)
        {
            var setting = new IncTryPushSetting();
            action.Do(r => r(setting));

            Func<ActionResult> defaultSuccess = () => View(composite.Parts[0]);
            var isActualAjax = isAjax.GetValueOrDefault(HttpContext.Request.IsAjaxRequest());
            if (isActualAjax)
                defaultSuccess = () => IncodingResult.Success();
            var success = setting.SuccessResult ?? defaultSuccess;

            Func<IncWebException, ActionResult> defaultError = (ex) => View(composite.Parts[0]);
            if (isActualAjax)
                defaultError = (ex) => IncodingResult.Error((ModelStateDictionary) ModelState);
            var error = setting.ErrorResult ?? defaultError;

            if (!ModelState.IsValid)
                return error(IncWebException.For(string.Empty, string.Empty));

            try
            {
                push(composite);
                return success();
            }
            catch (IncWebException exception)
            {
                foreach (var pairError in exception.Errors)
                {
                    foreach (var errorMessage in pairError.Value)
                        ModelState.AddModelError(pairError.Key, errorMessage);
                }

                return error(exception);
            }
        }
        
        #region Nested classes

        protected class IncTryPushSetting
        {
            #region Properties

            public Func<ActionResult> SuccessResult { get; set; }

            public Func<IncWebException, ActionResult> ErrorResult { get; set; }

            #endregion
        }

        #endregion
        
    }
}