using Incoding.Core.Data;
using Incoding.Core.Quality;
using JetBrains.Annotations;

namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    public class SpecWithPersistenceSpecification<TEntity> where TEntity : class, IEntity, new()
    {
        #region Static Fields

        [ThreadStatic]
        protected static PersistenceSpecification<TEntity> persistenceSpecification;

        

        #endregion
            
        #region Fields

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true)]
        Establish establish = () =>
        {
            persistenceSpecification = new PersistenceSpecification<TEntity>();
        };

        #endregion
    }
}