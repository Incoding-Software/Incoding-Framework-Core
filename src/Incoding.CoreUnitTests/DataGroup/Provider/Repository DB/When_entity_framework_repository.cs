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
        Because of = () =>
                     {
                         //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<IncDbContext>());
                         var dbContext = new IncDbContext(typeof(DbEntity).Assembly);
                         dbContext.Database.AutoTransactionsEnabled = false;
                         //dbContext.Configuration.LazyLoadingEnabled = true;
                         repository = new EntityFrameworkRepository(dbContext);
                         repository.Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}