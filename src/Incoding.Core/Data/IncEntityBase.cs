using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Core.Quality;

namespace Incoding.Core.Data
{
    #region << Using >>

    #endregion

    public abstract class IncEntityBase : IEntity
    {
        #region IEntity Members

        [IgnoreCompare("Design fixed")]
        public virtual object Id { get; set; }

        #endregion

        #region Equals

        public override int GetHashCode()
        {
            return Id.ReturnOrDefault(r => r.GetHashCode(), 0);
        }

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && GetHashCode().Equals(obj.GetHashCode());
        }

        #endregion

        public static bool operator ==(IncEntityBase left, IncEntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IncEntityBase left, IncEntityBase right)
        {
            return !Equals(left, right);
        }
    }
}