using Incoding.Data.Raven.Provider;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using System.Linq;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Listeners;

    #endregion

    [Subject(typeof(RavenDbRepository)), Isolated]
    public class When_rave_db_repository : Behavior_repository
    {
        #region Fake classes

        public class NoStaleQueriesListener : IDocumentQueryListener
        {
            #region Implementation of IDocumentQueryListener

            public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
            {
                queryCustomization.WaitForNonStaleResults();
            }

            #endregion
        }

        #endregion

        #region Establish value

        protected static IRepository BuildRepository()
        {
            var docSession = new DocumentStore
            {
                Url = ConfigurationManager.ConnectionStrings["IncRealRavenDb"].ConnectionString,
                DefaultDatabase = "IncTest",
            };
            docSession.Conventions.AllowQueriesOnId = true;
            docSession.Conventions.MaxNumberOfRequestsPerSession = 1000;
            docSession.Initialize();
            docSession.RegisterListener(new NoStaleQueriesListener());

            var session = docSession.OpenSession();
            return new RavenDbRepository(session);
        }

        #endregion

        Because of = () =>
                     {
                         GetRepository = () => BuildRepository().Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;

        private It should_be_performance_get_by_id = () =>
        {
            var repository = GetRepository();
            var id = repository.Query<DbEntityQuery>().First().Id;
            Pleasure.Do(i => repository.GetById<DbEntityQuery>(id), 1000)
                    .ShouldBeLessThan(10);
        };
    }
}