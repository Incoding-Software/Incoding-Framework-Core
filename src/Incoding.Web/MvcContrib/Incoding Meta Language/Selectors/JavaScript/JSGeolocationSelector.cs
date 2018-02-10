using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.JavaScript
{
    public class JSGeolocationSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        public JSGeolocationSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public JSCoordsSelector CurrentPosition
        {
            get
            {
                AddProperty("geolocation");
                AddProperty("currentPosition");
                return new JSCoordsSelector(this);
            }
        }

        #endregion
    }
}