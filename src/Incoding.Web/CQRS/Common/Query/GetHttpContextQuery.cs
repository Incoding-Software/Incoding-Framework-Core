using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Incoding.CQRS
{
    public class GetHttpContextQuery : QueryBase<HttpContext>
    {
        protected override HttpContext ExecuteResult()
        {
            return new HttpContextAccessor().HttpContext;
        }
    }
}