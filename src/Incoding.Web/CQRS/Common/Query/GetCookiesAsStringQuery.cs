using System.Diagnostics.CodeAnalysis;
using Incoding.Core.CQRS.Core;
using Incoding.Core;
using JetBrains.Annotations;

namespace Incoding.Web.CQRS.Common.Query
{
    #region << Using >>

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly]
    public class GetCookiesAsStringQuery : QueryBase<string>
    {
        #region Constructors

        public GetCookiesAsStringQuery() { }

        public GetCookiesAsStringQuery(string key)
        {
            Key = key;
        }

        #endregion

        #region Properties

        public string Key { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            return Dispatcher.Query(new GetHttpContextQuery()).With(r => r.Request)
                              .With(r => r.Cookies[Key])
                              .With(r => r);
        }
    }
}