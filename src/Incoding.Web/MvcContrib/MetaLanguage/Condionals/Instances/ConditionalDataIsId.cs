using System;
using System.Linq.Expressions;
using Incoding.Core.Extensions;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class ConditionalDataIsId<TModel> : ConditionalBase
    {
        #region Fields

        readonly string property;

        #endregion

        #region Constructors

        public ConditionalDataIsId(Expression<Func<TModel, object>> property, bool and)
                : base(ConditionalOfType.DataIsId.ToString(), and)
        {
            this.property = ReflectionExtensions.GetMemberName(property);
        }

        #endregion

        public override object GetData()
        {
            return new
                       {
                               this.type, 
                               this.property, 
                               this.inverse, 
                               this.and
                       };
        }
    }
}