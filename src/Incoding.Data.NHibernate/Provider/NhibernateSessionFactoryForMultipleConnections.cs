using System;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using FluentNHibernate.Infrastructure;
using NHibernate;
using NHibernate.Cfg;

namespace Incoding.Data.NHibernate
{
    public class NhibernateSessionFactoryForMultipleConnections : INhibernateSessionFactory
    {
        #region Fields

        readonly Lazy<ISessionFactory> sessionFactory;

        public static Func<string, DbConnection> GetConnection { get; set; }

        #endregion

        #region INhibernateSessionFactory Members

        public ISession Open(string connectionString)
        {
            //if (!string.IsNullOrWhiteSpace(connectionString))
            //    session.Connection.ConnectionString = connectionString;
            var sessionFactoryValue = sessionFactory.Value;
            ISession session;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                session = sessionFactoryValue.WithOptions()
                    .Connection(GetConnection(connectionString)).OpenSession();
            }
            else
                session = sessionFactoryValue.OpenSession();

            return session;
        }

        #endregion

        ////ncrunch: no coverage start

        #region Constructors

        public NhibernateSessionFactoryForMultipleConnections(FluentConfiguration fluentConfiguration)
        {
            sessionFactory = new Lazy<ISessionFactory>(fluentConfiguration.BuildSessionFactory);
        }

        public NhibernateSessionFactoryForMultipleConnections(Func<Configuration> atOnce, string path)
        {
            Configuration cfg = null;
            IFormatter serializer = new BinaryFormatter(new NetStandardSerialization.SurrogateSelector(), new StreamingContext());



            if (path != null && File.Exists(path))
            {
                try
                {
                    using (Stream stream = File.OpenRead(path))
                        cfg = serializer.Deserialize(stream) as Configuration;
                }
                catch (Exception ex)
                {

                }
            }

            if (cfg == null)
            {
                cfg = atOnce();
                if (path != null)
                {
                    try
                    {
                        using (Stream stream = File.OpenWrite(path))
                            serializer.Serialize(stream, cfg);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            this.sessionFactory = new Lazy<ISessionFactory>(Fluently.Configure(cfg).BuildSessionFactory);
        }

        ////ncrunch: no coverage end
        [Obsolete("Please use ctor with Fluent Configuration ")]
        public NhibernateSessionFactoryForMultipleConnections(ISessionFactory sessionFactory)
        {
            this.sessionFactory = new Lazy<ISessionFactory>(() => sessionFactory);
        }

        #endregion
    }
}