using Microsoft.EntityFrameworkCore;

namespace Incoding.Data
{
    public interface IEFClassMap
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}