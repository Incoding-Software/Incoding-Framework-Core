using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageWithDsl
    {
        IHtmlHelper Html { get; }

        IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelectorExtend> selector);

        IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelectorExtend selector);

        IIncodingMetaLanguageCallbackInstancesDsl WithName<T>(Expression<Func<T, object>> memberName);

        IIncodingMetaLanguageCallbackInstancesDsl WithName(string name);

        IIncodingMetaLanguageCallbackInstancesDsl WithId<T>(Expression<Func<T, object>> memberId);

        IIncodingMetaLanguageCallbackInstancesDsl WithId(string id);

        IIncodingMetaLanguageCallbackInstancesDsl WithClass(string @class);

        IIncodingMetaLanguageCallbackInstancesDsl WithClass(B @class);

        IIncodingMetaLanguageCallbackInstancesDsl WithSelf(Func<JquerySelectorExtend, JquerySelectorExtend> self);

        IIncodingMetaLanguageCallbackInstancesDsl Self();
    }
}