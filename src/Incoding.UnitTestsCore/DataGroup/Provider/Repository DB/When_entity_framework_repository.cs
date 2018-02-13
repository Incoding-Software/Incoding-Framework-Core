using Incoding.Data.EF.Provider;
using Microsoft.EntityFrameworkCore;

namespace Incoding.UnitTest
{
    #region << Using >>
    
    using Incoding.Data;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(EntityFrameworkRepository)), Isolated]
    public class When_entity_framework_repository : Behavior_repository
    {
        private Because of = () =>
                     {
                         GetRepository = () => new EntityFrameworkRepository(MSpecAssemblyContext.EFFluent()).Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}