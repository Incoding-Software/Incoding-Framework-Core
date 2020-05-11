using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Incoding.Core.Extensions
{
    #region << Using >>

    #endregion

    public static class StringExtensions
    {
        #region Factory constructors

        public static string[] SplitByFirst(this string str, char splitChar)
        {
            int index = str.IndexOf(splitChar);
            if (index < 0 || index > str.Length)
                return new string[]{};
            var first = str.Substring(0, index);
            var last = str.Substring(index + 1, str.Length - index - 1);
            return new [] { first, last };
        }
        public static bool EqualsWithInvariant(this string left, string right)
        {
            return left.Equals(right, StringComparison.InvariantCultureIgnoreCase);
        }

        [DebuggerStepThrough, StringFormatMethod("format")]
        public static string F(this string format, params object[] param)
        {
            return string.Format(format, param);
        }

        
        public static string AsString(this IEnumerable<object> values, string separate)
        {
            return string.Join(separate, values);
        }

        
        public static bool IsGuid(this string value)
        {
            Guid outGuid;
            return Guid.TryParse(value, out outGuid);
        }

        
        public static string ToSafeJSArgument(this string value)
        {
            return value.Replace("'", "\\'");
        }


       

        #endregion
    }
}