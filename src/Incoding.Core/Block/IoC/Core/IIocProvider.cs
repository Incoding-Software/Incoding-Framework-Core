using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Incoding.Core.Block.IoC.Core
{
    #region << Using >>

    #endregion

    public interface IIoCProvider : IDisposable
    {
        #region Methods

        void Eject<TInstance>();

        void Forward<TNew>([NotNull] TNew newInstance) where TNew : class;

        //[Obsolete("Please use TryGet because performance issues")]
        //TInstance Get<TInstance>([NotNull] Type type) where TInstance : class;

        IEnumerable<TInstance> GetAll<TInstance>([NotNull] Type typeInstance);

        TInstance TryGet<TInstance>() where TInstance : class;

        TInstance TryGet<TInstance>([NotNull] Type type) where TInstance : class;

        TInstance TryGetByNamed<TInstance>([NotNull] object named) where TInstance : class;

        #endregion
    }
}