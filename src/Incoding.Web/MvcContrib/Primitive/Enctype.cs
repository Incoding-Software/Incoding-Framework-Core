using System.ComponentModel;

namespace Incoding.Mvc.MvcContrib.Primitive
{
    public enum Enctype
    {
        [Description("application/x-www-form-urlencoded")]
        ApplicationXWWWFormUrlEncoded,

        [Description("multipart/form-data")]
        MultipartFormData,

        [Description("text/plain")]
        TextPlan
    }
}