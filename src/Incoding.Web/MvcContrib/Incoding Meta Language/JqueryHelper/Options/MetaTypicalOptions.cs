using System.Collections.Generic;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Options
{
    #region << Using >>

    #endregion

    public abstract class MetaTypicalOptions : Dictionary<string, object>
    {
        #region Constructors

        protected MetaTypicalOptions(MetaTypicalOptions @default)
        {            
            foreach (var item in @default)
                Add(item.Key, item.Value);
        }

        protected MetaTypicalOptions() { }

        #endregion
    }
}