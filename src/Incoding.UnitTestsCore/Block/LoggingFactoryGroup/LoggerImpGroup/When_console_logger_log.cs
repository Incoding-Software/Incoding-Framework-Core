﻿using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Block.Logging.Loggers;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.UnitTests.MSpec;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConsoleLogger))]
    public class When_console_logger_log : Context_Logger
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => { logger = new ConsoleLogger(); };

        Because of = () => { exception = Catch.Exception(() => logger.Log(new LogMessage(Pleasure.Generator.String(), null, null))); };

        It should_be_log_without_exception = () => exception.ShouldBeNull();
    }
}