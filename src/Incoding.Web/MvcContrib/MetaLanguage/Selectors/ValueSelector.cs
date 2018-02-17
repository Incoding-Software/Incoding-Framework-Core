using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
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