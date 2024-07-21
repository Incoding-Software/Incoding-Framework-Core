using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Block.Scheduler.Persistence;
using Incoding.Core.Block.Scheduler.Query;
using JetBrains.Annotations;

namespace Incoding.Data.NHibernate.Provider
{
        [UsedImplicitly, ExcludeFromCodeCoverage]
        public class DelayToSchedulerNhMap : NHibernateEntityMap<DelayToScheduler>
        {
            #region Constructors

            protected DelayToSchedulerNhMap()
            {
                IdGenerateByGuid(r => r.Id);
                Id(r => r.Id).GeneratedBy.Assigned();
                MapEscaping(r => r.Command).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Type).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Priority);
                MapEscaping(r => r.UID);
                MapEscaping(r => r.Description).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Status).CustomType<DelayOfStatus>();
                MapEscaping(r => r.StartsOn);
                MapEscaping(r => r.CreateDt);
                Component(r => r.Option, part =>
                {
                    part.Map(r => r.Async, "Option_Async");
                    part.Map(r => r.TimeOut, "Option_TimeOut");
                });
                Component(r => r.Recurrence, part =>
                {
                    part.Map(r => r.EndDate, "Recurrence_EndDate");
                    part.Map(r => r.RepeatCount, "Recurrence_RepeatCount");
                    part.Map(r => r.RepeatDays, "Recurrence_RepeatDays").CustomType<GetRecurrencyDateQuery.DayOfWeek?>();
                    part.Map(r => r.RepeatInterval, "Recurrence_RepeatInterval");
                    part.Map(r => r.StartDate, "Recurrence_StartDate");
                    part.Map(r => r.Type, "Recurrence_Type").CustomType<GetRecurrencyDateQuery.RepeatType?>();
                });
            }

            #endregion
        }
    
}