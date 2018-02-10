using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Incoding.Block;
using Incoding.Block.Logging;
using Incoding.Data.Block;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;

namespace Incoding.Data
{
    public static class NhibernatePersist
    {
        public static Configuration CreateDefault(Type entityType, string connectionString, long version)
        {
            Configuration cfg = null;
            string path = Path.Combine(AppContext.BaseDirectory, string.Format("fluently_{0}_{1}.cfg", version,
#if DEBUG
                    new string(connectionString.ToCharArray().Select(r => Char.IsLetterOrDigit(r) ? r : 'a').ToArray())
#else
                ""
#endif
                ));
            
            cfg = Create(path, () => Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012//.Raw("use_sql_comments", "true")
                    .ConnectionString(connectionString))
                .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(entityType.Assembly)
                    .Add(typeof(DelayToSchedulerMap.Map)))
                .ExposeConfiguration(r => new SchemaUpdate(r).Execute(false, true))
#if DEBUG
                .ExposeConfiguration(x =>
                {
                    x.SetInterceptor(new SqlStatementInterceptor());
                })
#endif
                .BuildConfiguration());

            return cfg;
        }

        public static Configuration Create(string path, Func<Configuration> config)
        {
            Configuration cfg = null;


            IFormatter serializer = new BinaryFormatter();
            if (File.Exists(path))
            {
                try
                {
                    using (Stream stream = File.OpenRead(path))
                        cfg = serializer.Deserialize(stream) as Configuration;
                }
                catch (Exception exLoad)
                {
                    LoggingFactory.Instance.LogException(LogType.Trace, exLoad);
                }
            }
            if (cfg == null)
            {
                cfg = config();

                try
                {
                    using (Stream stream = File.OpenWrite(path))
                        serializer.Serialize(stream, cfg);
                }
                catch (Exception exSave)
                {
                    LoggingFactory.Instance.LogException(LogType.Trace, exSave);
                }
            }
            return cfg;
        }
    }

    public class SqlStatementInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.WriteLine(sql.ToString());
            return base.OnPrepareStatement(sql);
        }
    }
}