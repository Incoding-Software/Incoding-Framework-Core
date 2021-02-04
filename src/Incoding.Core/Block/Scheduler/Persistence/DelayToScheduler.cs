
using System;
using System.Linq;
using System.Linq.Expressions;
using Incoding.Core.Block.Scheduler.Query;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Incoding.Core.Block.Scheduler.Persistence
{
    #region << Using >>

    #endregion

    public partial class DelayToScheduler : IncEntityBase
    {
        #region Constructors

        public DelayToScheduler()
        {
            Id = Guid.NewGuid().ToString();           
        }

        #endregion

        public class OptionOfDelay
        {
            public OptionOfDelay()
            {
                Async = false;
                TimeOut = 5.Seconds().Milliseconds;
            }

            public bool Async { get; set; }

            public int TimeOut { get; set; }
        }

        public abstract class Sort
        {
            public class Default : OrderSpecification<DelayToScheduler>
            {
                public override Action<AdHocOrderSpecification<DelayToScheduler>> SortedBy()
                {
                    return specification => specification.OrderBy(r => r.Status)
                                                         .OrderBy(r => r.Priority)
                                                         .OrderByDescending(r => r.StartsOn);
                }
            }
        }

        public abstract class Where
        {
            public class ByUID : Specification<DelayToScheduler>
            {
                #region Fields

                readonly string uid;

                #endregion

                #region Constructors

                public ByUID(string uid)
                {
                    this.uid = uid;
                }

                #endregion

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => scheduler.UID == this.uid;
                }
            }

            public class AvailableStartsOn : Specification<DelayToScheduler>
            {
                readonly DateTime date;

                public AvailableStartsOn(DateTime date)
                {
                    this.date = date;
                }

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    var faultAbove = this.date.AddMinutes(2);
                    return scheduler => scheduler.StartsOn <= faultAbove;
                }
            }

            public class ByAsync : Specification<DelayToScheduler>
            {
                private readonly bool @async;

                public ByAsync(bool @async)
                {
                    this.async = async;
                }

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => scheduler.Option.Async == this.async;
                }
            }

            public class ByStatus : Specification<DelayToScheduler>
            {
                #region Fields

                readonly DelayOfStatus[] status;

                #endregion

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => status.Contains(scheduler.Status);
                }

                #region Constructors

                public ByStatus(DelayOfStatus status)
                        : this(new[] { status }) { }

                public ByStatus(DelayOfStatus[] status)
                {
                    this.status = status;
                }

                #endregion
            }
        }

        #region Properties

        public new virtual string Id { get; protected set; }

        public virtual string Command { get; set; }

        
        public virtual string Type { get; set; }

        public virtual int Priority { get; set; }

        public virtual DelayOfStatus Status { get; set; }

        public virtual string Description { get; set; }

        public virtual string UID { get; set; }

        public virtual DateTime StartsOn { get; set; }

        public virtual GetRecurrencyDateQuery Recurrence { get; set; }

        public virtual OptionOfDelay Option { get; set; }

        public virtual DateTime? CreateDt { get; set; }

        public static bool RemoveAfterSuccess { get; set; }

        #endregion

        #region Nested classes

        

        #endregion
    }
}