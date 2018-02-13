using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;
    using MongoDB.Bson.Serialization.Attributes;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore, ItemIsReference = true)]
    public class DbEntityQueryAsItem : IncEntityBase
    {
        #region Constructors

        public DbEntityQueryAsItem()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public new virtual Guid Id { get; set; }

        [JsonIgnore, BsonIgnore]
        public virtual DbEntityQuery Parent { get; set; }

        #endregion

        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityQueryAsItem>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                DefaultReference(r => r.Parent);
            }

            #endregion
        }
        */
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityQueryAsItem>
        {
            public override void OnModel(EntityTypeBuilder<DbEntityQueryAsItem> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                entity.HasOne(r => r.Parent);
            }
        }

        #endregion
    }
}