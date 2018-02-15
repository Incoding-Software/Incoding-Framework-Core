using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Incoding.Core;
using Incoding.Core.Extensions;
using Incoding.Core.Maybe;
using Incoding.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Mvc.MvcContrib.Core
{
    #region << Using >>

    #endregion

    public class IncodingResult : ActionResult
    {
        #region Properties

        public object Data { get; set; }

        #endregion

        public override string ToString()
        {
            return Data.ToJsonString();
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            bool? isMultiPart = context.HttpContext.Request.ContentType?.Contains("multipart/form-data");
            var isIe = EqualsExtensions.IsAnyEqualsIgnoreCase("IE");
            response.ContentType = isMultiPart.GetValueOrDefault() && isIe ? "text/html" : "application/json";
            using (StreamWriter tw = new StreamWriter(response.Body))
            {
                await tw.WriteAsync(Data.ToJsonString());
            }
        }

        public override void ExecuteResult(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var isMultiPart = context.HttpContext.Request.ContentType.Contains("multipart/form-data");
            var isIe = EqualsExtensions.IsAnyEqualsIgnoreCase("IE");
            response.ContentType = isMultiPart && isIe ? "text/html" : "application/json";
            using (StreamWriter tw = new StreamWriter(response.Body))
            {
                tw.Write(Data.ToJsonString());
            }
        }
        
        #region Factory constructors

        public static IncodingResult Error(object data = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new IncodingResult
                   {
                           Data = new JsonData(false, data, string.Empty, statusCode)
                   };
        }

        public static IncodingResult Error(ModelStateDictionary modelState)
        {
            var errorData = modelState
                    .Select(valuePair => new JsonModelStateData
                                         {
                                                 name = valuePair.Key,
                                                 isValid = !modelState[valuePair.Key].Errors.Any(),
                                                 errorMessage = modelState[valuePair.Key].Errors
                                                                                         .FirstOrDefault()
                                                                                         .ReturnOrDefault(s => s.ErrorMessage, string.Empty)
                                         });

            return new IncodingResult
                   {
                           Data = new JsonData(false, errorData, string.Empty, HttpStatusCode.OK)
                   };
        }

        public static IncodingResult RedirectTo(string url)
        {
            Guard.NotNullOrWhiteSpace("url", url);
            return new IncodingResult
                   {
                           Data = new JsonData(true, string.Empty, url, HttpStatusCode.OK)
                   };
        }

        public static IncodingResult Success(object data = null)
        {
            return new IncodingResult
                   {
                           Data = new JsonData(true, data, string.Empty, HttpStatusCode.OK),
                   };
        }

        public static IncodingResult Success(Func<object, HelperResult> text)
        {
            string data = text
                    .Invoke(null)
                    .ToString()
                    .Replace(Environment.NewLine, string.Empty);
            return Success(data);
        }

        #endregion

        #region Nested classes

        public class JsonData
        {
            #region Constructors

            public JsonData(bool success, object data, string redirectTo, HttpStatusCode statusCode)
            {
                this.success = success;
                this.data = data;
                this.redirectTo = redirectTo;
                this.statusCode = statusCode;
            }

            #endregion

            #region Properties

            public bool success { get; set; }

            public object data { get; set; }

            public string redirectTo { get; set; }

            public HttpStatusCode statusCode { get; set; }

            #endregion
        }

        public class JsonModelStateData
        {
            #region Properties

            public string name { get; set; }

            public bool isValid { get; set; }

            public string errorMessage { get; set; }

            #endregion
        }

        #endregion
    }
}