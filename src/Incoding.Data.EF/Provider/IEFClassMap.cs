using Microsoft.EntityFrameworkCore;

namespace Incoding.Data.EF.Provider
{
    public interface IEFClassMap
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}