using System;
using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Extensions;
using Incoding.Core.Quality;
using JetBrains.Annotations;

namespace Incoding.Core.CQRS.Common.Response
{
    #region << Using >>

    #endregion

    [Obsolete("Please use Bool or Bool?")]
    public class IncBoolResponse : IncStructureResponse<bool>
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, false), ExcludeFromCodeCoverage]
        public IncBoolResponse() { }

        public IncBoolResponse(bool value)
                : base(value) { }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return obj.IsReferenceEquals(this) &&
                   ((IncStructureResponse<bool>)obj).Value.Equals(Value);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        public static implicit operator IncBoolResponse(bool s)
        {
            return new IncBoolResponse(s);
        }

        public static implicit operator bool(IncBoolResponse s)
        {
            return s.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator ==(bool left, IncBoolResponse right)
        {
            return new IncBoolResponse(left).Equals(right);
        }

        public static bool operator !=(bool left, IncBoolResponse right)
        {
            return !new IncBoolResponse(left).Equals(right);
        }

        public static bool operator ==(IncBoolResponse left, bool right)
        {
            return new IncBoolResponse(right).Equals(left);
        }

        public static bool operator !=(IncBoolResponse left, bool right)
        {
            return !new IncBoolResponse(right).Equals(left);
        }
    }
}