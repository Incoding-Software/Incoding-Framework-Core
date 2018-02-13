using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    public class DbEntityQueryAsReference : IncEntityBase
    {
        #region Constructors

        public DbEntityQueryAsReference()
        {
            this.Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public new virtual Guid Id { get; set; }

        #endregion

        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityQueryAsReference>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
            }

            #endregion
        }*/

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityQueryAsReference>
        {
            public override void OnModel(EntityTypeBuilder<DbEntityQueryAsReference> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedNever();
            }
        }

        #endregion
    }
}