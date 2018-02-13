using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incoding.Data.EF.Provider
{
    public abstract class EFClassMap<TEntity> : IEFClassMap where TEntity : class
    {
        #region Api Methods
        
        public virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModel(modelBuilder.Entity<TEntity>());
        }

        public abstract void OnModel(EntityTypeBuilder<TEntity> entity);

        #endregion
    }
}