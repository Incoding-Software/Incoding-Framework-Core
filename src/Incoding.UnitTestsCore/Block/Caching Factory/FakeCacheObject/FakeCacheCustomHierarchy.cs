using Incoding.Core.Block.Caching.Core;

namespace Incoding.UnitTest.Block
{
    public class FakeCacheCustomHierarchy : ICacheKey
    {
        #region ICacheKey Members

        public string GetName()
        {
            return GetType().Name;
        }

        #endregion
    }
}