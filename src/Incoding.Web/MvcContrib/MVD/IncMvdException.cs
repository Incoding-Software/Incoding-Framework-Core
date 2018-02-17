using System;

namespace Incoding.Web.MvcContrib
{
    public class IncMvdException : Exception
    {
        #region Constructors

        public IncMvdException(string message)
                : base(message) { }

        #endregion
    }
}