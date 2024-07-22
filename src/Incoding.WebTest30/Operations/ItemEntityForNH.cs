using System;
using System.Linq.Expressions;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.Data.NHibernate;
using Incoding.Data.NHibernate.Provider;

namespace Incoding.WebTest30.Operations
{
    public class ItemEntity : IncEntityBase, IName
    {
        public new virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public class ItemMap : NHibernateEntityMap<ItemEntity>
        {
            protected ItemMap()
            {
                Id(r => r.Id).GeneratedBy.GuidComb().Not.Nullable();
                MapEscaping(r => r.Name);
            }

            //public override void OnModel(EntityTypeBuilder<ItemEntity> entity)
            //{
            //    entity.HasKey(r => r.Id);
            //    entity.Property(r => r.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newid()");
            //    entity.Property(r => r.Name);
            //}
        }

        public abstract class Where
        {
            public class ByStringLongerThan : Specification<ItemEntity>
            {
                private readonly int _i;

                public ByStringLongerThan(int i)
                {
                    _i = i;
                }

                public override Expression<Func<ItemEntity, bool>> IsSatisfiedBy()
                {
                    return entity => entity.Name.Length > _i;
                }
            }
        }

        public abstract class Order
        {
            public class ByName : OrderSpecification<ItemEntity>
            {
                public override Action<AdHocOrderSpecification<ItemEntity>> SortedBy()
                {
                    return specification => specification.OrderBy(r => r.Name);
                }
            }
        }
    }

    public interface IName : IEntity
    {
        string Name { get; set; }
    }

    public class NameEntity : IName
    {
        public object Id { get; set; }
        public string Name { get; set; }
    }
    public class NameSpec<T> : Specification<T> where T : IName
    {
        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            Specification<ItemEntity> spec = new ItemEntity.Where.ByStringLongerThan(10);
            spec.And(new NameSpec<ItemEntity>());
            return name => name.Name != null;
        }
    }
}