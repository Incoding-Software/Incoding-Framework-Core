﻿using System.Data;
using Incoding.Core.Data;
using Incoding.Core.Extensions;

namespace Incoding.UnitTest
{
    #region << Using >>
    
    using Incoding.Data;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behavior_unit_of_work_factory
    {
        #region Establish value

        protected static IsolationLevel isolated = IsolationLevel.ReadCommitted;

        protected static string connectionString = Pleasure.Generator.String();

        protected static bool isFlush = Pleasure.Generator.Bool();

        protected static IUnitOfWork unitOfWork;

        #endregion

        It should_be_not_null = () => unitOfWork.ShouldNotBeNull();

        It should_be_not_null_session = () => unitOfWork.TryGetValue("session").ShouldNotBeNull();
    }
}