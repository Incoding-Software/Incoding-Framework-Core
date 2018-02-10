using Microsoft.EntityFrameworkCore;

namespace Incoding.Data
{
    public interface IEntityFrameworkSessionFactory : ISessionFactory<DbContext>
    {        
    }
}