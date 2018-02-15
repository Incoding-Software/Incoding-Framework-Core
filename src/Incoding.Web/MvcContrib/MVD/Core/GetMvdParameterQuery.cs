using System.Collections.Specialized;
using System.Web;
using Incoding.Core.CQRS.Core;
using Incoding.CQRS;

namespace Incoding.Mvc.MvcContrib.MVD.Core
{
    #region << Using >>

    #endregion

    public sealed class GetMvdParameterQuery : QueryBase<GetMvdParameterQuery.Response>
    {
        public NameValueCollection Params { get; set; }

        protected override Response ExecuteResult()
        {
            bool incIsModel;
            bool.TryParse(Params["incIsModel"], out incIsModel);

            bool isValidate;
            bool.TryParse(Params["incValidate"], out isValidate);

            bool isCompositeArray;
            bool.TryParse(Params["incIsCompositeAsArray"], out isCompositeArray);

            var contentType = Params["incContentType"];
            return new Response()
                   {
                           Type = Params["incType"] ?? Params["incTypes"],
                           IsModel = incIsModel,
                           View = HttpUtility.UrlDecode(Params["incView"]),
                           IsValidate = isValidate,
                           IsCompositeArray = isCompositeArray,
                           ContentType = string.IsNullOrWhiteSpace(contentType) ? "img" : contentType,
                           FileDownloadName = Params["incFileDownloadName"] ?? string.Empty,
                   };
        }

        public class Response
        {
            public string Type { get; set; }

            public bool IsModel { get; set; }

            public string View { get; set; }

            public bool IsValidate { get; set; }

            public string ContentType { get; set; }

            public string FileDownloadName { get; set; }

            public bool IsCompositeArray { get; set; }
        }
    }
}