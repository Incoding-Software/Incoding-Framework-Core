using System;

namespace Incoding.Core.Block.Scheduler.Persistence
{
    public class OptionOfDelayAttribute : Attribute
    {
        public bool Async { get; set; }

        
        public int TimeOutOfMillisecond { get; set; }
    }
}