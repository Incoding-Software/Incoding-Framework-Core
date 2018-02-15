using System;

namespace Incoding.Core
{
    #region << Using >>

    #endregion

    [Serializable]
    public class IncFrameworkException : Exception
    {
        #region Constructors

        public IncFrameworkException(string message)
                : base(message) { }

        #endregion
    }
}