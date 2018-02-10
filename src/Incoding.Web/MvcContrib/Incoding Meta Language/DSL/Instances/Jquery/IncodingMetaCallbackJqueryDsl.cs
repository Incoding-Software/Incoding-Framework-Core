using System;
using Incoding.Extensions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances.Jquery
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackJqueryDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Properties

        [Obsolete("Please use Attr")]
        public IncodingMetaCallbackJqueryAttributesDsl Attributes { get { return Attr; } }
        
        public IncodingMetaCallbackJqueryAttributesDsl Attr { get { return new IncodingMetaCallbackJqueryAttributesDsl(this.plugInDsl); } }

        public IncodingMetaCallbackJqueryCssDsl Css { get { return new IncodingMetaCallbackJqueryCssDsl(this.plugInDsl); } }

        [Obsolete("Please use Dom")]
        public IncodingMetaCallbackJqueryManipulationDsl Manipulation { get { return new IncodingMetaCallbackJqueryManipulationDsl(this.plugInDsl); } }

        public IncodingMetaCallbackInsertDsl Dom { get { return new IncodingMetaCallbackInsertDsl(this.plugInDsl); } }

        #endregion

        #region Api Methods

        public IExecutableSetting PlugIn(string name, object options = null)
        {
            return this.plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Jquery_Plug_In.F(name, options.ReturnOrDefault(r => ObjectExtensions.ToJsonString(r), string.Empty))));
        }

        public IExecutableSetting Call(string name, params object[] args)
        {
            return this.plugInDsl.Registry(new ExecutableEvalMethod(name, args, "$(this.target)"));
        }

        #endregion
    }
}