using Incoding.Core.Block.Logging.Core;
using Incoding.Core.Block.Logging.Loggers;

namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Machine.Specifications;

    #endregion

    [Subject(typeof(FileLogger))]
    public class When_file_logger_log_with_template : Context_file_logger
    {
        #region Establish value

        static string thisIsCustomFormat;

        static string fileName = typeof(When_file_logger_log_with_template).Name;

        #endregion

        Because of = () =>
                         {
                             thisIsCustomFormat = "This is custom format";
                             logger = FileLogger
                                     .WithEachReplace(folderPath, () => fileName)
                                     .WithTemplate(logMessage => thisIsCustomFormat);

                             logger.Log(new LogMessage(string.Empty, null, null));
                         };

        It should_be_write_file_with_custom_format = () => GetContent(fileName).ShouldEqual(thisIsCustomFormat);
    }
}