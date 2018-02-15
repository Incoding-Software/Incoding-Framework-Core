using System;

namespace Incoding.Core.Block.Caching.Providers
{
    public class CacheOptions
    {
        public static readonly CacheOptions DefaultCacheOptions = new CacheOptions
        {
            ValidFor = TimeSpan.FromMinutes(10)
        };

        private TimeSpan? _validFor;

        public TimeSpan ValidFor
        {
            get => _validFor ?? DefaultCacheOptions.ValidFor;
            set => _validFor = value;
        }
    }
}