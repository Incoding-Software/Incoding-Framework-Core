﻿using System;
using Raven.Client;

namespace Incoding.Data.Raven.Provider
{
    #region << Using >>

    #endregion

    public class RavenDbSessionFactory : IRavenDbSessionFactory
    {
        #region Fields

        readonly Lazy<IDocumentStore> documentStore;

        #endregion

        #region Constructors

        public RavenDbSessionFactory(IDocumentStore documentStore)
        {
            this.documentStore = new Lazy<IDocumentStore>(documentStore.Initialize);
        }

        #endregion

        #region IRavenDbSessionFactory Members

        public IDocumentSession Open(string connection)
        {
            var currentSession = !string.IsNullOrWhiteSpace(connection)
                                     ? this.documentStore.Value.OpenSession(connection)
                                     : this.documentStore.Value.OpenSession();
            return currentSession;
        }

        #endregion
    }
}