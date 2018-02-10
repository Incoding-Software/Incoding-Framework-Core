﻿using System;
using Incoding.Block.IoC;
using Incoding.Mvc.MvcContrib.Template.Factory;
using Incoding.Mvc.MvcContrib.Template.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Template
{
    #region << Using >>

    #endregion

    public class MvcTemplate<TModel> : IDisposable
    {
        #region Static Fields

        readonly Lazy<ITemplateFactory> factory = new Lazy<ITemplateFactory>(() => IoCFactory.Instance.TryResolve<ITemplateFactory>() ?? new TemplateHandlebarsFactory());

        #endregion

        #region Fields

        readonly IHtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcTemplate(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        #endregion

        #region Api Methods

        public ITemplateSyntax<TModel> ForEach()
        {
            return factory.Value.ForEach<TModel>(this.htmlHelper);
        }

        public ITemplateSyntax<TModel> NotEach()
        {
            return factory.Value.NotEach<TModel>(this.htmlHelper);
        }

        #endregion

        #region Disposable

        public virtual void Dispose() { }

        #endregion
    }
}