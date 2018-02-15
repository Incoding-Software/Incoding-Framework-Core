using System.Data;
using Incoding.UnitTest.MSpecGroup;
using Microsoft.EntityFrameworkCore;

namespace Incoding.UnitTest
{
    #region << Using >>
    
    using Incoding.Block;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Raven.Client.Document;

    #endregion

    [Subject(typeof(DelayToScheduler))/*, Isolated*/]
    public class When_save_DelayToScheduler : SpecWithPersistenceSpecification<DelayToScheduler>
    {
        It should_be_ef = () => new PersistenceSpecification<DelayToScheduler>(PleasureForData.Factory()
                                                                                              .Create(IsolationLevel.ReadUncommitted, true, null))
                                        .VerifyMappingAndSchema();

        It should_be_nhibernate = () => new PersistenceSpecification<DelayToScheduler>()
                                                .VerifyMappingAndSchema();

        It should_be_raven_db = () => new PersistenceSpecification<DelayToScheduler>(When_persistence_specification_with_instance.BuildRavenDb(new DocumentStore
                                                                                                                  {
                                                                                                                          Url = "http://localhost:8090/",
                                                                                                                          DefaultDatabase = "IncTest",
                                                                                                                  }).Create(IsolationLevel.ReadCommitted, false))
                                              .VerifyMappingAndSchema();
    }
}