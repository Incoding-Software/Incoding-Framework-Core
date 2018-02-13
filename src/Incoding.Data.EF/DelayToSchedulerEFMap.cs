using System;
using System.Diagnostics.CodeAnalysis;
using Incoding.Block;
using Incoding.Quality;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.Data.EF
{
    public class DelayToSchedulerEFMap
    {
        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, false), ExcludeFromCodeCoverage]
        public class EfMap : Provider.EFClassMap<DelayToScheduler>
        {
            public override void OnModel(EntityTypeBuilder<DelayToScheduler> entity)
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id)
                    .ValueGeneratedNever();
                entity.OwnsOne(r => r.Option);
                entity.OwnsOne(r => r.Recurrence).Ignore(r => r.Result).Ignore(r => r.Setting);
            }
        }
    }
}