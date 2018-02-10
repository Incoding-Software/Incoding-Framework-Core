﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    #endregion

    [ExcludeFromCodeCoverage]
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase<DbContext>
    {
        #region Fields

        readonly IDbContextTransaction transaction;

        bool isWasCommit;

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWork(DbContext session, IsolationLevel level,bool isFlush)
                : base(session)
        {
            transaction = session.Database.BeginTransaction(level);
            
            if (!isFlush)
                session.ChangeTracker.AutoDetectChangesEnabled = false;
            repository = new EntityFrameworkRepository(session);
        }

        #endregion

        protected override void InternalFlush()
        {
            session.SaveChanges();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
            isWasCommit = true;
        }

        protected override void InternalSubmit()
        {
            if (!isWasCommit)
                transaction.Rollback();

            transaction.Dispose();
        }
    }
}