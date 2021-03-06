﻿using Incoding.Core.Block.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Core;
using Incoding.Data.EF.Provider;
using Incoding.Data.Mongo.Provider;
using Incoding.Data.Raven.Provider;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;
    using NCrunch.Framework;
    //using NHibernate;
    using Raven.Client.Linq;

    #endregion

    [Behaviors, Isolated]
    public class Behavior_repository
    {
        #region Fake classes

        class JoinManyFetchSpecification : FetchSpecification<DbEntityQuery>
        {
            public override Action<AdHocFetchSpecificationBase<DbEntityQuery>> FetchedBy()
            {
                var repository = GetRepository();
                if (repository.IsNH())
                    return specification => specification.JoinMany(dbEntity => dbEntity.Items, entity => entity.Parent);

                return specification => specification.Join(dbEntity => dbEntity.Items.Select(entity => entity.Parent));
            }
        }

        class JoinJoinFetchSpecification : FetchSpecification<DbEntityQueryAsItem>
        {
            public override Action<AdHocFetchSpecificationBase<DbEntityQueryAsItem>> FetchedBy()
            {
                var repository = GetRepository();
                if (repository.IsNH())
                    return specification => specification.Join(dbEntity => dbEntity.Parent, entity => entity.Reference);

                return specification => specification.Join(dbEntity => dbEntity.Parent.Reference);
            }
        }

        #endregion

        #region Query

        #region Fake classes

        class FakeJoinFetch : FetchSpecification<DbEntityQuery>
        {
            public override Action<AdHocFetchSpecificationBase<DbEntityQuery>> FetchedBy()
            {
                return specification => specification.Join(r => r.Reference);
            }
        }

        class DbEntityValueGreater0WhereSpec : Specification<DbEntityQuery>
        {
            public override Expression<Func<DbEntityQuery, bool>> IsSatisfiedBy()
            {
                return entity => entity.Value > 0;
            }
        }

        class DbEntityByIdWhereSpec : Specification<DbEntityQuery>
        {
            #region Fields

            readonly Guid id;

            #endregion

            #region Constructors

            public DbEntityByIdWhereSpec(Guid id)
            {
                this.id = id;
            }

            #endregion

            public override Expression<Func<DbEntityQuery, bool>> IsSatisfiedBy()
            {
                return entity => entity.Id == id;
            }
        }

        class RealDbEntityContainIdWhereSpec : Specification<DbEntityQuery>
        {
            #region Fields

            readonly Guid[] ids;

            readonly bool isRaven;

            #endregion

            #region Constructors

            public RealDbEntityContainIdWhereSpec(Guid[] ids, bool isRaven)
            {
                this.ids = ids;
                this.isRaven = isRaven;
            }

            #endregion

            public override Expression<Func<DbEntityQuery, bool>> IsSatisfiedBy()
            {
                return isRaven
                               ? (Expression<Func<DbEntityQuery, bool>>)(entity => entity.Id.In(ids))
                               : (entity => ids.Contains(entity.Id));
            }
        }

        class FakeOrderDescSpecification : OrderSpecification<DbEntityQuery>
        {
            public override Action<AdHocOrderSpecification<DbEntityQuery>> SortedBy()
            {
                return specification => specification
                                                .OrderByDescending(r => r.Value)
                                                .OrderBy(r => r.Value2);
            }
        }

        class FakeOrderSpecification : OrderSpecification<DbEntityQuery>
        {
            public override Action<AdHocOrderSpecification<DbEntityQuery>> SortedBy()
            {
                return specification => specification
                                                .OrderBy(r => r.Value)
                                                .OrderByDescending(r => r.Value2);
            }
        }

        #endregion

        It should_be_paginated = () =>
        {
            var repository = GetRepository();
            repository.Paginated<DbEntityQuery>(new PaginatedSpecification(1, 5))
                .Should(result =>
                {
                    result.TotalCount.ShouldEqual(10);
                    result.Items.Count.ShouldEqual(5);
                });
        };

        It should_be_paginated_with_fetch_and_order = () =>
        {
            var repository = GetRepository();
            repository.Paginated(new PaginatedSpecification(1, 5),
                    orderSpecification: new FakeOrderSpecification(),
                    fetchSpecification: new FakeJoinFetch(),
                    whereSpecification: new RealDbEntityContainIdWhereSpec(repository.Query<DbEntityQuery>()
                        .Take(7)
                        .ToList()
                        .Select(r => r.Id)
                        .ToArray(), repository is RavenDbRepository))
                .Should(result =>
                {
                    result.TotalCount.ShouldEqual(7);
                    if (!repository.IsNH())
                        result.Items.Count.ShouldEqual(5); // bug with nhibernate.

                    repository.Close(() => result.Items[0].Reference.ShouldNotBeNull());
                });
        };

        It should_be_query = () =>
        {
            var repository = GetRepository();
            repository.Query<DbEntityQuery>()
                .Count()
                .ShouldEqual(10);
        };

        It should_be_query_all_spec = () =>
        {
            var repository = GetRepository();
            repository.Query(paginatedSpecification: new PaginatedSpecification(1, 5),
                    orderSpecification: new FakeOrderSpecification(),
                    whereSpecification: new DbEntityValueGreater0WhereSpec(),
                    fetchSpecification: new FakeJoinFetch())
                .ToList()
                .Should(list =>
                {
                    if (!repository.IsNH())
                        list.Count.ShouldEqual(5); // bug with nhibernate.

                    foreach (var realDbEntity in list)
                        realDbEntity.Reference.ShouldNotBeNull();
                });
        };

        It should_be_query_order_descending = () =>
        {
            var repository = GetRepository();
            repository.Query(orderSpecification: new FakeOrderDescSpecification())
                .ToList()
                .Should(entities =>
                {
                    entities[0].Value.ShouldBeGreaterThan(entities[1].Value);
                    entities[1].Value.ShouldBeGreaterThan(entities[2].Value);
                });
        };

        It should_be_query_order = () =>
        {
            var repository = GetRepository();
            repository.Query(orderSpecification: new FakeOrderSpecification())
                .ToList()
                .Should(entities =>
                {
                    entities[0].Value.ShouldBeLessThan(entities[1].Value);
                    entities[1].Value.ShouldBeLessThan(entities[2].Value);
                    entities[2].Value.ShouldBeLessThan(entities[3].Value);
                });
        };

        // ReSharper disable RemoveToList.2
        It should_be_query_paginated = () =>
        {
            var repository = GetRepository();
            repository.Query<DbEntityQuery>(paginatedSpecification: new PaginatedSpecification(1, 5))
                .ToList()
                .Count
                .ShouldEqual(5);
        };

        // ReSharper restore RemoveToList.2
        It should_be_query_where_true = () =>
        {
            var repository = GetRepository();
            repository.Query(
                    whereSpecification: new DbEntityByIdWhereSpec(repository.Query<DbEntityQuery>().First().Id))
                .Count()
                .ShouldEqual(1);
        };

        It should_be_query_where_false = () =>
        {
            var repository = GetRepository();
            repository.Query(whereSpecification: new DbEntityByIdWhereSpec(Guid.NewGuid()))
                .Count()
                .ShouldEqual(0);
        };

        #endregion

        #region Crud

        It should_be_save = () =>
                            {
                                var repository = GetRepository();
                                var entity = new DbEntityAsGuid();
                                repository.Save(entity);
                                repository.Clear();

                                repository.GetById<DbEntityAsGuid>(entity.Id).ShouldNotBeNull();
                            };

        It should_be_saves = () =>
                             {
                                 var repository = GetRepository();
                                 var entity = new DbEntityAsGuid();
                                 var entity2 = new DbEntityAsGuid();
                                 repository.Saves(new[]
                                                  {
                                                          entity, 
                                                          entity2
                                                  });
                                 repository.Clear();

                                 repository.GetById<DbEntityAsGuid>(entity.Id).ShouldNotBeNull();
                                 repository.GetById<DbEntityAsGuid>(entity2.Id).ShouldNotBeNull();
                             };

        It should_be_save_with_reference = () =>
                                           {
                                               var repository = GetRepository();
                                               var entity = Pleasure.Generator.Invent<DbEntity>(dsl => dsl.GenerateTo(r => r.Reference));
                                               repository.Save(entity);
                                               repository.Clear();

                                               var dbEntity = repository.GetById<DbEntity>(entity.Id);
                                               dbEntity.ShouldEqualWeak(entity, dsl => dsl.NotNull(r => r.Reference));
                                               if (!repository.IsMongo())
                                                   repository.GetById<DbEntityReference>(dbEntity.Reference.Id).ShouldNotBeNull();
                                           };

        It should_be_save_with_many = () =>
                                      {
                                          var repository = GetRepository();
                                          var entity = Pleasure.Generator.Invent<DbEntity>(dsl => dsl.Callback(r =>
                                                                                                               {
                                                                                                                   var item = new DbEntityItem();
                                                                                                                   r.AddItem(item);
                                                                                                               }));
                                          repository.Save(entity);
                                          repository.Clear();

                                          var dbEntity = repository.GetById<DbEntity>(entity.Id);
                                          dbEntity.ShouldEqualWeak(entity, dsl => dsl.ForwardToAction(r => r.Items, r => r.Items.ShouldNotBeEmpty()));
                                          if (!repository.IsMongo())
                                              repository.GetById<DbEntityItem>(dbEntity.Items[0].Id).ShouldNotBeNull();
                                      };

        It should_be_save_or_update_with_new = () =>
                                               {
                                                   var repository = GetRepository();
                                                   var entity = new DbEntity();
                                                   repository.SaveOrUpdate(entity);
                                                   repository.Clear();

                                                   repository.GetById<DbEntity>(entity.Id).ShouldNotBeNull();
                                               };
        /*
        It should_be_get_provider = () =>
                                    {
                                        if (repository.IsNH())
                                            repository.GetProvider<ISession>().ShouldNotBeNull();
                                    };
*/It should_be_get_provider = () =>
                                    {
                                        var repository = GetRepository();
                                        if (!repository.IsNH())
                                            repository.GetProvider<IncDbContext>().ShouldNotBeNull();
                                    };

        It should_be_save_or_update_with_exist = () =>
                                                 {
                                                     var repository = GetRepository();
                                                     var entity = repository.Query<DbEntity>()
                                                                            .First();
                                                     Catch.Exception(() => repository.SaveOrUpdate(entity)).ShouldBeNull();
                                                     repository.Clear();

                                                     repository.GetById<DbEntity>(entity.Id).ShouldNotBeNull();
                                                 };

        It should_be_delete_all = () =>
                                  {
                                      var repository = GetRepository();
                                      Pleasure.Do10(i => repository.Save(new DbEntityAsGuid()));
                                      repository.Flush();

                                      repository.DeleteAll<DbEntityAsGuid>();
                                      repository.Clear();

                                      repository.Query<DbEntityAsGuid>().ShouldBeEmpty();
                                  };

        It should_be_delete_by_id = () =>
                                    {
                                        var repository = GetRepository();
                                        var entity = new DbEntityAsGuid();
                                        repository.Save(entity);
                                        repository.Flush();

                                        repository.Delete<DbEntityAsGuid>(entity.Id);
                                        repository.Flush();

                                        repository.GetById<DbEntityAsGuid>(entity.Id).ShouldBeNull();
                                    };

        It should_be_delete_by_ids = () =>
                                     {
                                         var repository = GetRepository();
                                         var ids = new Guid[10];
                                         Pleasure.Do10(i =>
                                                       {
                                                           var entity = new DbEntityAsGuid();
                                                           repository.Save(entity);
                                                           repository.Flush();
                                                           ids[i] = entity.Id;
                                                       });
                                         repository.Flush();
                                         repository.DeleteByIds<DbEntityAsGuid>(ids.Cast<object>().ToArray());
                                         repository.Clear();
                                         repository.Flush();
                                         //if(repository.IsEF())
                                         //    repository.GetProvider<IncDbContext>().Set<DbEntityAsGuid>().Local.Clear();

                                         ids.DoEach(o => repository.GetById<DbEntityAsGuid>(o).ShouldBeNull());
                                     };

        private It should_be_delete_by_ids_int = () =>
                                         {
                                             var repository = GetRepository();
                                             var ids = new int[10];
                                             Pleasure.Do10(i =>
                                                           {
                                                               var entity = new DbEntityByInt();
                                                               if (repository is MongoDbRepository)
                                                                   entity.Id = i;
                                                               repository.Save(entity);
                                                               repository.Flush();
                                                               ids[i] = entity.Id;
                                                           });

                                             repository.DeleteByIds<DbEntityByInt>(ids.Cast<object>().ToArray());
                                             repository.Flush();
                                             repository.Clear();

                                             ids.DoEach(o => repository.GetById<DbEntityByInt>(o).ShouldBeNull());
                                         };

        It should_be_delete_by_ids_with_special_name_column = () =>
                                                              {
                                                                  var repository = GetRepository();
                                                                  var ids = new object[10];
                                                                  Pleasure.Do10(i =>
                                                                                {
                                                                                    var entity = new DbEntityWithSpecificIdName();
                                                                                    repository.Save(entity);
                                                                                    repository.Flush();
                                                                                    ids[i] = entity.Id;
                                                                                });

                                                                  repository.DeleteByIds<DbEntityWithSpecificIdName>(ids.ToArray());
                                                                  repository.Flush();
                                                                  repository.Clear();

                                                                  ids.DoEach(o => repository.GetById<DbEntityWithSpecificIdName>(o).ShouldBeNull());
                                                              };

        #endregion

        #region Fetch

        It should_be_query_fetch = () =>
                                   {
                                       var repository = GetRepository();
                                       if (repository.IsMongo())
                                           return;

                                       var fetchMany = repository.Query(fetchSpecification: new JoinManyFetchSpecification())
                                                                 .ToList();

                                       var fetchJoinJoin = repository.Query(fetchSpecification: new JoinJoinFetchSpecification())
                                                                     .ToList();

                                       fetchMany.ShouldNotBeEmpty();
                                       fetchJoinJoin.ShouldNotBeEmpty();

                                       repository.Close(() =>
                                                        {
                                                            fetchMany.Should(list =>
                                                                             {
                                                                                 foreach (var realDbEntity in list.ToList())
                                                                                     realDbEntity.Items[0].Parent.ShouldNotBeNull();
                                                                             });

                                                            fetchJoinJoin.Should(list =>
                                                                                 {
                                                                                     foreach (var realDbEntity in list.ToList())
                                                                                         realDbEntity.Parent.Reference.ShouldNotBeNull();
                                                                                 });
                                                        });
                                   };

        #endregion

        #region Select

        It should_be_get_by_id = () =>
                                 {
                                     var repository = GetRepository();
                                     var entity = new DbEntityAsGuid();
                                     repository.Save(entity);
                                     repository.Flush();

                                     repository.GetById<DbEntityAsGuid>(entity.Id).ShouldNotBeNull();
                                 };

        It should_be_get_by_id_with_null = () =>
        {
            var repository = GetRepository();
            repository.GetById<DbEntityAsGuid>(null).ShouldBeNull();
        };

        It should_be_load_by_id = () =>
                                  {
                                      var repository = GetRepository();
                                      var entity = new DbEntityAsGuid();
                                      repository.Save(entity);

                                      repository.LoadById<DbEntityAsGuid>(entity.Id).ShouldNotBeNull();
                                  };

        It should_be_load_by_id_with_null = () =>
        {
            var repository = GetRepository();
            repository.LoadById<DbEntityAsGuid>(null).ShouldBeNull();
        };

        #endregion

        #region Establish value
        
        protected static Func<IRepository> GetRepository;

        #endregion

        It should_be_execute_sql = () =>
                                   {
                                       var repository = GetRepository();
                                       bool isRaven = repository is RavenDbRepository;
                                       bool isMongo = repository is MongoDbRepository;
                                       if (!isRaven && !isMongo)
                                       {
                                           var entity = new DbEntityAsGuid();
                                           repository.Save(entity);
                                           repository.Flush();

                                           repository.ExecuteSql("Delete from DbEntityAsGuid_Tbl Where Id = '{0}'".F(entity.Id));
                                           repository.Clear();

                                           repository.GetById<DbEntityAsGuid>(entity.Id).ShouldBeNull();
                                       }
                                   };
    }
}