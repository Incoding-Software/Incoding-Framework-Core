using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using FluentNHibernate.Infrastructure;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Cfg;

namespace Incoding.Data.NHibernate.Provider
{
    #region << Using >>

    #endregion

    public class NhibernateSessionFactory : INhibernateSessionFactory
    {
        #region Fields

        readonly Lazy<ISessionFactory> sessionFactory;

        #endregion

        #region INhibernateSessionFactory Members

        public ISession Open(string connectionString)
        {
            var session = sessionFactory.Value.OpenSession();
            if (!string.IsNullOrWhiteSpace(connectionString))
                session.Connection.ConnectionString = connectionString;

            return session;
        }

        #endregion

        ////ncrunch: no coverage start

        #region Constructors

        public NhibernateSessionFactory(FluentConfiguration fluentConfiguration)
        {
            sessionFactory = new Lazy<ISessionFactory>(fluentConfiguration.BuildSessionFactory);
        }

        public NhibernateSessionFactory(Func<Configuration> atOnce, string path)
        {
            Configuration cfg = null;
            
            if (path != null && File.Exists(path))
            {
                try
                {
                    using (FileStream stream = File.OpenRead(path))
                        cfg = ProtoBuf.Serializer.Deserialize<Configuration>(stream);
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
                        using (FileStream stream = File.Open(path, FileMode.Create))
                            ProtoBuf.Serializer.Serialize(stream, cfg);
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
        public NhibernateSessionFactory(ISessionFactory sessionFactory)
        {
            this.sessionFactory = new Lazy<ISessionFactory>(() => sessionFactory);
        }

        #endregion
    }
}