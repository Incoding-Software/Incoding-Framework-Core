using System;
using System.Net;
using Incoding.Core.Block.ExceptionHandling;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Incoding.Web.MvcContrib
{
    public class IncodingErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HandleExceptionAsync(context);
            context.ExceptionHandled = true;
        }

        private static void HandleExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            ExceptionHandlingFactory.Instance.Handler(exception);

            SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
        }

        private static void SetExceptionResult(
            ExceptionContext context,
            Exception exception,
            HttpStatusCode code)
        {
            context.Result = IncodingResult.Error(null, code);
        }
    }
}