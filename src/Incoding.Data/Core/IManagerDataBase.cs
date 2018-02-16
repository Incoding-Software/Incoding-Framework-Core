using System;

namespace Incoding.Data.Core
{
    #region << Using >>

    #endregion

    public interface IManagerDataBase
    {
        void Create();

        void Drop();

        void Update();

        bool IsExist();

        bool IsExist(out Exception outException);
    }
}