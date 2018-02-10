namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageWithDsl
    {
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