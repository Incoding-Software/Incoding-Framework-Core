using Incoding.Core.Data;
using Incoding.Core.Quality;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public abstract class EntityVm
    {
        #region Constructors

        protected EntityVm(IEntity entity)
        {
            Id = entity.Id.ToString();
            AssemblyQualifiedName = entity.GetType().AssemblyQualifiedName;
        }

        #endregion

        #region Properties

        [IgnoreCompare("Base class")]
        public string Id { get; private set; }

        [IgnoreCompare("Base class")]
        public string AssemblyQualifiedName { get; private set; }

        #endregion
    }
}