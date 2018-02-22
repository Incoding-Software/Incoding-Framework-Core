using System;
using System.Collections.Generic;
using Incoding.Core.Data;
using Incoding.Core.Quality;
using Incoding.Data;
using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GridUI.Persistance
{
    public class Product : IncEntityBase
    {
        #region Properties

        public new virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual bool IsSoldOut { get; set; }

        public virtual IList<User> Users{ get; set; }

        #endregion

        public class ProductMap : EFClassMap<Product>
        {
            public override void OnModel(EntityTypeBuilder<Product> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                entity.Property(r => r.Date);
                entity.Property(r => r.IsSoldOut);
                entity.Property(r => r.Name);
                entity.Property(r => r.Price);
                entity.HasMany(r => r.Users);
            }
        }
        
        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor)]
        public class ProductMap : NHibernateEntityMap<Product>
        {
            ////ncrunch: no coverage start

            #region Constructors

            protected ProductMap()
            {
                Id(r => r.Id).GeneratedBy.Guid();
                Map(r => r.Name).Length(100);
                Map(r => r.Price);
                Map(r => r.Date);
                Map(r => r.IsSoldOut);
                HasManyToMany(r => r.Users);
            }

            #endregion

            ////ncrunch: no coverage end
        }
        */
        #endregion
    }
}