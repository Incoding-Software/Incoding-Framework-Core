using System;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances.Jquery;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    #region << Using >>

    #endregion

    public class IncodingMetaLanguageCoreDsl : IIncodingMetaLanguageCoreDsl
    {
        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        protected IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaLanguageCoreDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugIn)
        {
            _htmlHelper = htmlHelper;
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackBindDsl Bind { get { return new IncodingMetaCallbackBindDsl(_htmlHelper, this.plugIn); } }

        public IncodingMetaCallbackInsertDsl Insert { get { return new IncodingMetaCallbackInsertDsl(this._htmlHelper, this.plugIn); } }

        public IExecutableSetting Break { get { return this.plugIn.Registry(new ExecutableBreak()); } }

        public IncodingMetaCallbackJqueryDsl JQuery { get { return new IncodingMetaCallbackJqueryDsl(_htmlHelper, this.plugIn); } }

        public IncodingMetaCallbackFuncDsl Func { get { return new IncodingMetaCallbackFuncDsl(_htmlHelper, this.plugIn); } }

        public IncodingMetaCallbackStoreApiDsl Store { get { return new IncodingMetaCallbackStoreApiDsl(this.plugIn); } }

        public IncodingMetaCallbackTriggerDsl Trigger { get { return new IncodingMetaCallbackTriggerDsl(this.plugIn); } }

        public IncodingMetaCallbackFormApiDsl Form { get { return new IncodingMetaCallbackFormApiDsl(this.plugIn); } }

        #endregion

        #region Api Methods

        [Obsolete("Will be remove on next version then use method Call instead")]
        public IExecutableSetting Eval(string code)
        {
            return this.plugIn.Registry(new ExecutableEval(code));
        }

        public IExecutableSetting Call(string funcName, params object[] args)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod(funcName, args, string.Empty));
        }

        #endregion
    }
}