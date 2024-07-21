using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NHibernate.Linq;

namespace Incoding.Data.NHibernate.Provider
{
    #region << Using >>

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class SqlFunctions
    {
        #region Factory constructors

        [LinqExtensionMethod("NEWID")]
        public static Guid NewID()
        {
            return Guid.NewGuid();
        }

        #endregion
    }
}