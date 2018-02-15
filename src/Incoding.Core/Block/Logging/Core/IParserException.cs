using System;

namespace Incoding.Core.Block.Logging.Core
{
    #region << Using >>

    #endregion

    public interface IParserException
    {
        string Parse(Exception exception);
    }
}