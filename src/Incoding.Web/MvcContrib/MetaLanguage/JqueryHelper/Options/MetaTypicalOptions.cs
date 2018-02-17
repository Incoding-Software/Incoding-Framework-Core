using System.Collections.Generic;

namespace Incoding.Web.MvcContrib
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