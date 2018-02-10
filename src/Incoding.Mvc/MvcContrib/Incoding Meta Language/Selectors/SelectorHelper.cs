using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Jquery;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors
{
    #region << Using >>

    #endregion

    public class SelectorHelper<TModel>
    {
        #region Api Methods

        public JquerySelectorExtend Name<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            return Selector.Jquery.Name<TModel,TProperty>(property);
        }

        public JquerySelectorExtend Id(Expression<Func<TModel, object>> property)
        {
            return Selector.Jquery.Id<TModel>(property);
        }

        public Selector QueryString(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.QueryString<TModel>(property);
        }

        public Selector HashQueryString(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.HashQueryString<TModel>(property);
        }

        public Selector Cookie(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.Cookie<TModel>(property);
        }

        #endregion
    }
}