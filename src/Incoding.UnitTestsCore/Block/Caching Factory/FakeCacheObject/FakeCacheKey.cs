﻿using Incoding.Core.Block.Caching.Core;
using Incoding.Core;
using JetBrains.Annotations;

namespace Incoding.UnitTest.Block
{
    [Fake]
    public class FakeCacheKey : ICacheKey
    {
        #region Fields

        readonly string secretKey;
        
        #endregion

        #region Constructors

        public FakeCacheKey() { }

        public FakeCacheKey(string secretKey)
        {
            this.secretKey = secretKey;
        }

        #endregion

        #region Properties

        public bool IsReadyToExpires { get; set; }

        [UsedImplicitly]
        protected string ProtectedProperty { get; set; }

        #endregion

        #region ICacheKey Members

        public string GetName()
        {
            return this.secretKey
                       .If(r => !string.IsNullOrWhiteSpace(r))
                       .ReturnOrDefault(r => r, nameof(FakeCacheKey));
        }

        #endregion
    }
}