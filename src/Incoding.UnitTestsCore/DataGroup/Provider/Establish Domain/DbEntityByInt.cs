using Incoding.Core.Data;
using Incoding.Core.Quality;
using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;

    #endregion

    public class DbEntityByInt : IncEntityBase
    {
        #region Properties

        public new virtual int Id { get; set; }

        #endregion

        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityByInt>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Increment();
            }

            #endregion
        }
        */
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityByInt>
        {
            public override void OnModel(EntityTypeBuilder<DbEntityByInt> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
            }
        }

        #endregion
    }
}