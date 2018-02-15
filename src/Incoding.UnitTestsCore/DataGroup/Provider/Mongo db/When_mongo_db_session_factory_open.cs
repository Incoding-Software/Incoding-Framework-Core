using Incoding.Data.Mongo.Provider;
using MongoDB.Driver;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(MongoDbSessionFactory)), Isolated]
    public class When_mongo_db_session_factory_open
    {
        #region Establish value

        static MongoDbSessionFactory mongo;

        static MongoDatabaseWrapper session;

        #endregion

        Establish establish = () => { mongo = new MongoDbSessionFactory(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString); };

        Because of = () => { session = mongo.Open("mongodb://localhost:27017/IncDb"); };

        It should_be_open = () => session.Instance.DatabaseNamespace.DatabaseName.ShouldEqual("IncDb");
    }
}