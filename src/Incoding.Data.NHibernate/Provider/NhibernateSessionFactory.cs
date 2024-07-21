using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using FluentNHibernate.Infrastructure;
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
            //if (!string.IsNullOrWhiteSpace(connectionString))
            //    session.Connection.ConnectionString = connectionString;

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
            IFormatter serializer = new  BinaryFormatter(new NetStandardSerialization.SurrogateSelector(), new StreamingContext());



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
        public NhibernateSessionFactory(ISessionFactory sessionFactory)
        {
            this.sessionFactory = new Lazy<ISessionFactory>(() => sessionFactory);
        }

        #endregion
    }
}