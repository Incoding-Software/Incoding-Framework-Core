using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Incoding.Web.CQRS.Common.Query
{
    public class GetHttpContextQuery : QueryBase<HttpContext>
    {
        protected override HttpContext ExecuteResult()
        {
            return new HttpContextAccessor().HttpContext;
        }
    }
}