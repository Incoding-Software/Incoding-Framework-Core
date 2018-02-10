using Incoding.Data.Block;
using Microsoft.EntityFrameworkCore;

namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.Extensions;

    #endregion

    [Serializable]
    public class IncDbContext : DbContext
    {
        private readonly DbContextOptions<IncDbContext> _options;

        #region Fields

        readonly List<Type> mapsTypes;

        #endregion

        #region Constructors

        public IncDbContext(DbContextOptions<IncDbContext> options, Assembly mapAssembly)
                : base(options)
        {
            _options = options;
            this.mapsTypes = mapAssembly.GetTypes()
                                        .Where(r => r.IsImplement(typeof(EFClassMap<>)) &&
                                                    !r.IsInterface &&
                                                    !r.IsAbstract)
                                        .ToList();
            this.mapsTypes.AddRange(typeof(DelayToSchedulerMap).Assembly.GetTypes()
                .Where(r => r.IsImplement(typeof(EFClassMap<>)) &&
                            !r.IsInterface &&
                            !r.IsAbstract)
                .ToList());
        }

        public IncDbContext(DbContextOptions<IncDbContext> options)
                : this(options, Assembly.GetCallingAssembly()) { }

        
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var mapsType in this.mapsTypes)
            {
                var map = Activator.CreateInstance(mapsType) as IEFClassMap;
                map.OnModelCreating(modelBuilder);
            }
        }
    }
}