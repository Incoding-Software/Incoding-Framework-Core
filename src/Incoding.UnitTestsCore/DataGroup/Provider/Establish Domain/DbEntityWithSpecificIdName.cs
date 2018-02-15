using Incoding.Core.Data;
using Incoding.Core.Quality;
using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Machine.Specifications.Annotations;

    #endregion

    public class DbEntityWithSpecificIdName : IncEntityBase
    {
        #region Constructors

        public DbEntityWithSpecificIdName()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Properties

        public new virtual string Id { get; set; }

        #endregion

        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityWithSpecificIdName>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id)
                        .GeneratedBy
                        .Assigned()
                        .Column("NotId");
            }

            #endregion
        }
        */
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityWithSpecificIdName>
        {
            public override void OnModel(EntityTypeBuilder<DbEntityWithSpecificIdName> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
            }
        }

        #endregion
    }
}