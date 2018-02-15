namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using Incoding.Data;

    #endregion

    public static class PleasureForData
    {
        [ThreadStatic]
        public static Func<IUnitOfWorkFactory> Factory;
        
    }
}