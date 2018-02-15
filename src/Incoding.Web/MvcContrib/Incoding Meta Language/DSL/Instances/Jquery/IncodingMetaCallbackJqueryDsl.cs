using System;
using Incoding.Core.Extensions;
using Incoding.Core.Maybe;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances.Jquery
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackJqueryDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;
        readonly IHtmlHelper _htmlHelper;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            _htmlHelper = htmlHelper;
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Properties

        [Obsolete("Please use Attr")]
        public IncodingMetaCallbackJqueryAttributesDsl Attributes { get { return Attr; } }
        
        public IncodingMetaCallbackJqueryAttributesDsl Attr { get { return new IncodingMetaCallbackJqueryAttributesDsl(_htmlHelper, this.plugInDsl); } }

        public IncodingMetaCallbackJqueryCssDsl Css { get { return new IncodingMetaCallbackJqueryCssDsl(_htmlHelper, this.plugInDsl); } }

        [Obsolete("Please use Dom")]
        public IncodingMetaCallbackJqueryManipulationDsl Manipulation { get { return new IncodingMetaCallbackJqueryManipulationDsl(_htmlHelper, this.plugInDsl); } }

        public IncodingMetaCallbackInsertDsl Dom { get { return new IncodingMetaCallbackInsertDsl(this._htmlHelper, this.plugInDsl); } }

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