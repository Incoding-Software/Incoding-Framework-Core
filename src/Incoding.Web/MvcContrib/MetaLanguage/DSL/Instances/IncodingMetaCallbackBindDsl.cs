﻿using System;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackBindDsl
    {
        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackBindDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugIn)
        {
            _htmlHelper = htmlHelper;
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting Attach(Func<IIncodingMetaLanguageEventBuilderDsl, IIncodingMetaLanguageEventBuilderDsl> action)
        {
            string meta = action(new IncodingMetaLanguageDsl(_htmlHelper, string.Empty))
                    .AsHtmlAttributes()["incoding"].ToString();

            return this.plugIn.Registry(new ExecutableBind("attach", meta, string.Empty));
        }

        public IExecutableSetting DetachAll()
        {
            return Detach(string.Empty);
        }

        public IExecutableSetting Detach(JqueryBind bind)
        {
            return Detach(bind.ToStringLower());
        }

        public IExecutableSetting Detach(string bind)
        {
            return this.plugIn.Registry(new ExecutableBind("detach", string.Empty, bind));
        }

        #endregion
    }
}