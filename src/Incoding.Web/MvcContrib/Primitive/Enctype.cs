using System.ComponentModel;

namespace Incoding.Web.MvcContrib
{
    public enum Enctype
    {
        [Description("application/x-www-form-urlencoded")]
        ApplicationXwwwFormUrlEncoded ,

        [Description("multipart/form-data")]
        MultipartFormData,

        [Description("text/plain")]
        TextPlan
    }
}