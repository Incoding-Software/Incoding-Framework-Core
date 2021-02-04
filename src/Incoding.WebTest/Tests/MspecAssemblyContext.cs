using System.Globalization;
using System.Threading;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Incoding.Data.NHibernate;
using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Incoding.WebTest.Tests
{
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
        
        #endregion

        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
            //StartNh(true);
            //StartEF(/*NhibernateFluent*/EFFluent, false);
            //CachingFactory.Instance.Initialize(init => init.WithProvider(new EmptyCachedProvider()));

            //var serviceProvider = new ServiceCollection()
            //    .AddTransient<ICSPEUserConfig.IInterface,EmptyUserValues>()
            //    .BuildServiceProvider();
            //var t = serviceProvider.GetService<ICSPEUserConfig.IInterface>();

            //IoCFactoryTestEx.Stub(IoCFactory.Instance, mock => mock.Setup(r => r.TryGet<ICSPEUserConfig.IInterface>()).Returns(new EmptyUserValues()));
            //IoCFactory.Instance.Initialize(init => init.WithProvider(
            //    new TestMSDependencyInjectionIoCProvider(serviceProvider)
            //    //new MSDependencyInjectionIoCProvider(new DefaultServiceProviderFactory().CreateServiceProvider(new ServiceCollection()))
            //    ));
        }


        public void OnAssemblyComplete() { }

        #endregion
    }
}