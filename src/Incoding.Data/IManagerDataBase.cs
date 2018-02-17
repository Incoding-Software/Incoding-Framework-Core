using System;

namespace Incoding.Data
{
    public interface IManagerDataBase
    {
        void Create();

        void Drop();

        void Update();

        bool IsExist();

        bool IsExist(out Exception outException);
    }
}