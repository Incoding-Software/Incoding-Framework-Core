﻿using System.Collections.Generic;

namespace Incoding.Web.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Merge(this IDictionary<string, object> src, object dest)
        {
            var dictionaryFromAnonymous = AnonymousHelper.ToDictionary(dest);
            Core.Extensions.DictionaryExtensions.Merge(src, dictionaryFromAnonymous);
        }
    }
}