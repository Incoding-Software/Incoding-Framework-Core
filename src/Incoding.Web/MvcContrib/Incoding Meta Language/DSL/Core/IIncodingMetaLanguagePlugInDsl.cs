using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables.Core;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core
{
    public interface IIncodingMetaLanguagePlugInDsl
    {
        IExecutableSetting Registry(ExecutableBase callback);
    }
}