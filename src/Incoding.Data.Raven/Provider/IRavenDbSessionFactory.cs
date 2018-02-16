using Incoding.Data.Core;
using Raven.Client;

namespace Incoding.Data.Raven.Provider
{
    #region << Using >>

    #endregion

    public interface IRavenDbSessionFactory : ISessionFactory<IDocumentSession> { }
}