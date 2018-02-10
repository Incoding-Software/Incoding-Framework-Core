using System.Collections.Generic;

namespace Incoding.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Merge(this IDictionary<string, object> src, object dest)
        {
            var dictionaryFromAnonymous = AnonymousHelper.ToDictionary(dest);
            src.Merge(dictionaryFromAnonymous);
        }
    }
}