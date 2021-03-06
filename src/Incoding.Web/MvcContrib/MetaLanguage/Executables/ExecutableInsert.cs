using System.Collections.Generic;
using Incoding.Core.Extensions;
using Incoding.Core;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ExecutableInsert : ExecutableBase
    {
        const string insertType = "insertType";

        #region Constructors

        public ExecutableInsert(string insertType, string property, string template, bool prepare, string result)
        {
            if (!string.IsNullOrWhiteSpace(template))
                this.Set("template", template);

            if (!string.IsNullOrWhiteSpace(property))
                this["property"] = property;

            if (!string.IsNullOrWhiteSpace(result))
                this["result"] = result;

            if (prepare)
                this["prepare"] = true;

            this["insertType"] = insertType;
        }

        #endregion

        public override Dictionary<string, string> GetErrors()
        {
            if (string.IsNullOrWhiteSpace(this[insertType].With(r => r.ToString())))
            {
                return new Dictionary<string, string>()
                       {
                               { insertType, "Insert can be empty. Please choose Html/Text/Append or any type" }
                       };
            }

            return base.GetErrors();
        }
    }
}