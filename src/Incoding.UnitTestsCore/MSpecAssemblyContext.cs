using System;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Utilities;
using Incoding.Data;
using Incoding.Data.EF.Provider;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
//using NHibernate;

namespace Incoding.UnitTest
{
    #region << Using >>

    #region << Using >>

    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    //using FluentNHibernate.Cfg;
    //using FluentNHibernate.Cfg.Db;
    
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    //using NHibernate.Tool.hbm2ddl;

    #endregion

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly]
    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region Static Fields

        private static IConfigurationRoot _config;

        private static IConfigurationRoot Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                }
                return _config;
            }
        }

        #endregion

        #region Factory constructors
        /*
        public class MyLoggerFactory : INHibernateLoggerFactory
        {
            public INHibernateLogger LoggerFor(string keyName)
            {
                return new MyNoLoggingLogger();
            }

            public INHibernateLogger LoggerFor(Type type)
            {
                return new MyNoLoggingLogger();
            }
        }
        */

        //private static IncDbContext efContext;

        public static Func<IncDbContext> EFFluent = () =>
        {
            //if (efContext == null)
            //{
            var connectionString = Config.GetConnectionString("IncRealEFDb");
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<IncDbContext>()
                .UseSqlServer(connectionString);
            dbContextOptionsBuilder.ConfigureWarnings(r => r.Ignore(RelationalEventId.AmbientTransactionWarning));
            var efContext = new IncDbContext(dbContextOptionsBuilder.Options, typeof(DbEntity).Assembly);
            //bool deleted = efContext.Database.EnsureDeleted();
            //bool created = efContext.Database.EnsureCreated();
            //}
            return efContext;
        };
        /*
        public static FluentConfiguration NhibernateFluent()
        {
            //NHibernateLogger.SetLoggersFactory(new MyLoggerFactory());
            var connectionString = Config.GetConnectionString("IncRealNhibernateDb");
            return Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                                                .ConnectionString(connectionString)
                                                .ShowSql())                                                
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))                    
                    .Mappings(configuration => configuration.FluentMappings
                                                            .Add(typeof(DelayToSchedulerMap.EfMap))
                                                            .AddFromAssembly(typeof(DbEntity).Assembly));
        }
        */
        #endregion

        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
            StartEF(/*NhibernateFluent*/EFFluent, false);
        }

        public static void StartEF(Func<IncDbContext> dbContext, bool reloadDb = true)
        {
            PleasureForData.Factory = () => BuildEFSessionFactory(dbContext, reloadDb);
        }

        public static IUnitOfWorkFactory BuildEFSessionFactory(Func<IncDbContext> dbContext, bool reloadDb = false)
        {
            try
            {
                var context = dbContext();
                if (reloadDb)
                {
                    context.Database.EnsureDeleted();
                }

                context.Database.EnsureCreated();

                return new EntityFrameworkUnitOfWorkFactory(new EntityFrameworkSessionFactory((str) => context));
            }

            ////ncrunch: no coverage start
            catch (Exception e)
            {
                Clipboard.Copy("Exception in  build configuration {0}".F(e));
                return null;
            }

            ////ncrunch: no coverage end      
        }

        public void OnAssemblyComplete() { }

        #endregion
    }
    /*
    public class MyNoLoggingLogger : INHibernateLogger
    {
        public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
        {
            
        }

        public bool IsEnabled(NHibernateLogLevel logLevel)
        {
            return true;
        }
    }
    */
    ////ncrunch: no coverage end
}