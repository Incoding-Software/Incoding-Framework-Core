using Incoding.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data.EF.Provider
{
    public interface IEntityFrameworkSessionFactory : ISessionFactory<DbContext>
    {        
    }
}