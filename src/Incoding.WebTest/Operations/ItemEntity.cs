using System;
using Incoding.Data;
using Incoding.Data.EF.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.WebTest.Operations
{
    public class ItemEntity : IncEntityBase
    {
        public new Guid Id { get; set; }

        public string Name { get; set; }

        public class ItemMap : EFClassMap<ItemEntity>
        {
            public override void OnModel(EntityTypeBuilder<ItemEntity> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newid()");
                entity.Property(r => r.Name);
            }
        }
    }
}