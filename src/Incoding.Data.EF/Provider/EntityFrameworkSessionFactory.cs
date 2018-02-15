using System;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data.EF.Provider
{
    #region << Using >>

    #endregion

    public class EntityFrameworkSessionFactory : IEntityFrameworkSessionFactory
    {
        #region Static Fields

        //[ThreadStatic]
        //static DbContext currentSession;

        #endregion

        #region Fields

        readonly Func<string, DbContext> createDb;

        #endregion

        #region Constructors

        public EntityFrameworkSessionFactory(Func<string, DbContext> createDb)
        {
            this.createDb = createDb;
        }

        #endregion

        #region IEntityFrameworkSessionFactory Members

        //public DbContext Open(string connectionString)
        //{
        //    currentSession = this.createDb();
        //    if (!string.IsNullOrWhiteSpace(connectionString))
        //        currentSession.Database.Connection.ConnectionString = connectionString;
        //    return currentSession;
        //}
        
        #endregion

        public DbContext Open(string connectionString)
        {
            return createDb(connectionString);
        }
    }
}