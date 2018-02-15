using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Core.Extensions;
using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Jquery;
using Incoding.Mvc.MvcContrib.MVD;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackInsertDsl
    {
        #region Constructors

        public IncodingMetaCallbackInsertDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugIn)
        {
            _htmlHelper = htmlHelper;
            this.plugIn = plugIn;
        }

        #endregion

#if DEBUG
        [Obsolete("Please use selector as argument", false), ExcludeFromCodeCoverage]
#endif
#if !DEBUG
        [Obsolete("Please use selector as argument", true), UsedImplicitly, ExcludeFromCodeCoverage]
#endif
        public IncodingMetaCallbackInsertDsl WithTemplate(string selector)
        {
            throw new ArgumentException("Argument should be type of Selector", "selector");
        }

        //[Obsolete("Please use WithTemplateByUrl/WithTemplateByView")]
        internal IncodingMetaCallbackInsertDsl WithTemplate([NotNull] Selector selector)
        {
            this.insertTemplateSelector = selector;
            return this;
        }

        ////[Obsolete("Please use WithTemplateByUrl/WithTemplateByView")]
        //internal IncodingMetaCallbackInsertDsl WithTemplate([NotNull] JquerySelectorExtend selector)
        //{
        //    return WithTemplate(selector as Selector);
        //}

        ////[Obsolete("Suggest use ONLY WithTemplateByUrl")]
        //internal IncodingMetaCallbackInsertDsl WithTemplateById([NotNull, HtmlAttributeValue("id")] string id)
        //{
        //    return WithTemplate(id.ToId() as Selector);
        //}

        public IncodingMetaCallbackInsertDsl WithTemplateByUrl([NotNull] string url)
        {
            if (url.StartsWith("||"))
                throw new ArgumentException("Please use Url instead of Selector", "url");

            if (url.StartsWith("~"))
                throw new ArgumentException("Please use Url instead of path to View", "url");

            return WithTemplate(url.ToAjaxGet());
        }

        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByUrl(Func<UrlDispatcher, string> evaluated)
        {
            var dispatcher = new UrlDispatcher(new UrlHelper(_htmlHelper.ViewContext));
            return WithTemplateByUrl(evaluated(dispatcher));
        }

        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByView([AspMvcPartialView, NotNull] string view)
        {
            return WithTemplateByUrl((Func<UrlDispatcher, string>) (r => r.AsView(view)));
        }

        public IncodingMetaCallbackInsertDsl Prepare()
        {
            prepare = true;
            return this;
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Remove()
        {
            return this.plugIn.Core(_htmlHelper).JQuery.Call("remove");
        }

        /// <summary>
        ///     Remove all child nodes of the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Empty()
        {
            return this.plugIn.Core(_htmlHelper).JQuery.Call("empty");
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Detach()
        {
            return this.plugIn.Core(_htmlHelper).JQuery.Call("detach");
        }

        /// <summary>
        ///     Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting Wrap()
        {
            return this.plugIn.Core(_htmlHelper).JQuery.Call("wrap", this.content);
        }

        /// <summary>
        ///     Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting WrapAll()
        {
            return this.plugIn.Core(_htmlHelper).JQuery.Call(this.content);
        }

        [Obsolete("Please use On with Selector.Result.For<T>(r=>r.Prop)", false)]
        public IncodingMetaCallbackInsertDsl For<TModel>(Expression<Func<TModel, object>> property)
        {
            insertProperty = ReflectionExtensions.GetMemberName(property);
            return this;
        }

        public IncodingMetaCallbackInsertDsl Use(Func<object, HelperResult> text)
        {
            return Use(new ValueSelector(Selector.FromHelperResult(text)));
        }

        public IncodingMetaCallbackInsertDsl Use(Selector setContent)
        {
            content = setContent;
            return this;
        }

        /// <summary>
        ///     Set the data of every matched element through After.
        /// </summary>
        public IExecutableSetting After()
        {
            return InternalInsert("after");
        }

        public IExecutableSetting Val()
        {
            return InternalInsert("val");
        }

        public IExecutableSetting Before()
        {
            return InternalInsert("before");
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Append()
        {
            return InternalInsert("append");
        }

        public IExecutableSetting Prepend()
        {
            return InternalInsert("prepend");
        }

        /// <summary>
        ///     Set the data of every matched element through Html.
        /// </summary>
        public IExecutableSetting Html()
        {
            return InternalInsert("html");
        }

        /// <summary>
        ///     Set the data of every matched element through Text.
        /// </summary>
        public IExecutableSetting Text()
        {
            return InternalInsert("text");
        }

        IExecutableSetting InternalInsert(string method)
        {
            return plugIn.Registry(new ExecutableInsert(method, insertProperty, insertTemplateSelector, prepare, content));
        }

        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        string insertProperty = string.Empty;

        string insertTemplateSelector = string.Empty;

        bool prepare;

        string content = Selector.Result;

        #endregion
    }
}