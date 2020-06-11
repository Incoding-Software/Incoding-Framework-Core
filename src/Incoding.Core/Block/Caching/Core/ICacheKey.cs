using System.Diagnostics.CodeAnalysis;

namespace Incoding.Core.Block.Caching.Core
{
    #region << Using >>

    #endregion

    /// <summary>
    /// Caching Key interface
    /// </summary>
    public interface ICacheKey
    {
        #region Methods

        /// <summary>
        /// Unique key
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "False positive. Because usage func very native")]
        string GetName();

        #endregion
    }
}