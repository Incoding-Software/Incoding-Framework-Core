using Incoding.Data.EF.Provider;
using Microsoft.EntityFrameworkCore;

namespace Incoding.UnitTest
{
    #region << Using >>
    
    using Incoding.Data;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityFrameworkUnitOfWorkFactory))]
    public class When_entity_framework_unit_of_work_factory : Behavior_unit_of_work_factory
    {
        Establish establish = () =>
                              {
                                  var db = Pleasure.MockAsObject<DbContext>();
                                  var sessionFactory = Pleasure.MockStrictAsObject<IEntityFrameworkSessionFactory>(mock => mock.Setup(r => r.Open(connectionString)).Returns(db));
                                  unitOfWork = new EntityFrameworkUnitOfWorkFactory(sessionFactory).Create(isolated, true, connectionString);
                              };

        Because of = () => { };

        Behaves_like<Behavior_unit_of_work_factory> verify;
    }
}