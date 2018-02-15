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
    using Machine.Specifications.Annotations;
    using MongoDB.Bson.Serialization.Attributes;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore, ItemIsReference = true)]
    public class DbEntityItem : IncEntityBase
    {
        #region Constructors

        public DbEntityItem()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new virtual Guid Id { get; set; }

        [BsonIgnore]
        public virtual DbEntity Parent { get; set; }

        #endregion

        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityItem>
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
        public class EfMap : EFClassMap<DbEntityItem>
        {
            public override void OnModel(EntityTypeBuilder<DbEntityItem> entity)
            {
                entity.HasKey(r => r.Id);
            }
        }
        #endregion
    }
}