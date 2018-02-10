using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors
{
    #region << Using >>

    #endregion

    public class ValueSelector : Selector
    {
        public ValueSelector(object value)
                : this("||value*{0}||".F(value)) { }

        protected ValueSelector(Selector selector)
                : base(selector) { }
    }
}