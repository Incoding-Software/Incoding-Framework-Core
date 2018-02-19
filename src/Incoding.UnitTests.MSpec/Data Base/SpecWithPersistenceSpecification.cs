using System;
using Incoding.Core.Data;
using Incoding.Core.Quality;
using JetBrains.Annotations;
using Machine.Specifications;

namespace Incoding.UnitTests.MSpec
{
    #region << Using >>

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