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
    public class User : IncEntityBase
    {
        #region Properties

        public new virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual IList<Product> Products { get; set; }


        #endregion

        public class UserMap : EFClassMap<User>
        {
            public override void OnModel(EntityTypeBuilder<User> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                entity.Property(r => r.FirstName);
                entity.Property(r => r.LastName);
                entity.HasMany(r => r.Products);
            }
        }
        #region Nested classes
        /*
        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor)]
        public class UserMap : NHibernateEntityMap<User>
        {
            ////ncrunch: no coverage start

            #region Constructors

            protected UserMap()
            {
                Id(r => r.Id).GeneratedBy.Guid();
                Map(r => r.FirstName);
                Map(r => r.LastName);
                HasManyToMany(r => r.Products);
            }

            #endregion

            ////ncrunch: no coverage end
        }
        */
        #endregion
    }
}