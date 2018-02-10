using System;
using Incoding.Block.IoC;
using Incoding.Extensions;
using Incoding.Maybe;
using Incoding.Mvc.MvcContrib.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Options;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Primitive;
using Incoding.Mvc.MvcContrib.Template;
using Incoding.Mvc.MvcContrib.Template.Factory;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib.IncHtmlHelper
{
    public class IncodingHtmlHelper
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        #endregion

        ////ncrunch: no coverage start

        #region Constructors

        public IncodingHtmlHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        #endregion

        static TagBuilder CreateScript(string id, HtmlType type, string src, HtmlString content)
        {
            var routeValueDictionary = new RouteValueDictionary(new { type = type.ToLocalization() });
            if (!string.IsNullOrWhiteSpace(src))
                routeValueDictionary.Merge(new { src });
            if (!string.IsNullOrWhiteSpace(id))
                routeValueDictionary.Merge(new { id });

            return CreateTag(HtmlTag.Script, content, routeValueDictionary);
        }

        static TagBuilder CreateInput(string value, string type, object attributes)
        {
            var routeValueDictionary = AnonymousHelper.ToDictionary(attributes);
            routeValueDictionary.Merge(new { value, type });
            var input = CreateTag(HtmlTag.Input, HtmlString.Empty, routeValueDictionary);

            return input;
        }

        internal static TagBuilder CreateTag(HtmlTag tag, HtmlString content, RouteValueDictionary attributes)
        {
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.InnerHtml.AppendHtml(content.ReturnOrDefault(r => r, HtmlString.Empty));
            tagBuilder.MergeAttributes(attributes, true);

            return tagBuilder;
        }

        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global

        ////ncrunch: no coverage start

        #region Static Fields

        public static Selector DropDownTemplateId = "incodingDropDownTemplate".ToId();

        public static JqueryAjaxOptions DropDownOption = new JqueryAjaxOptions(JqueryAjaxOptions.Default);

        public static BootstrapOfVersion BootstrapVersion = BootstrapOfVersion.v2;

        public static B Def_Label_Class  = B.Col_xs_5;

        public static B Def_Control_Class = B.Col_xs_7;

        
        
        #endregion

        ////ncrunch: no coverage end

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global

        ////ncrunch: no coverage end

        #region Api Methods

        public IHtmlContent Script([PathReference] string src)
        {
            var script = CreateScript(string.Empty, HtmlType.TextJavaScript, src, HtmlString.Empty);
            return script;
        }

        [Obsolete("Please use Template")]
        public MvcScriptTemplate<TModel> ScriptTemplate<TModel>(string id)
        {
            return new MvcScriptTemplate<TModel>(this.htmlHelper, id);
        }

        public MvcTemplate<TModel> Template<TModel>()
        {
            return new MvcTemplate<TModel>(this.htmlHelper);
        }

        public IHtmlContent Link([PathReference] string href)
        {
            var tagBuilder = CreateTag(HtmlTag.Link, HtmlString.Empty, new RouteValueDictionary());
            tagBuilder.MergeAttribute(HtmlAttribute.Href.ToStringLower(), href);
            tagBuilder.MergeAttribute(HtmlAttribute.Rel.ToStringLower(), "stylesheet");
            tagBuilder.MergeAttribute(HtmlAttribute.Type.ToStringLower(), HtmlType.TextCss.ToLocalization());

            return tagBuilder;
        }

        public IHtmlContent Button(string value, object attributes = null)
        {
            var button = CreateTag(HtmlTag.Button, new HtmlString(value), AnonymousHelper.ToDictionary(attributes));
            return button;
        }

        public IHtmlContent Submit(string value, object attributes = null)
        {
            var submit = CreateInput(value, HtmlInputType.Submit.ToStringLower(), attributes);
            submit.TagRenderMode = TagRenderMode.SelfClosing;
            return submit;
        }

        public IHtmlContent Img(string src, object attributes = null)
        {
            return Img(src, HtmlString.Empty, attributes);
        }

        public IHtmlContent Img(string src, HtmlString content, object attributes = null)
        {
            var routeValueDictionary = AnonymousHelper.ToDictionary(attributes);
            routeValueDictionary.Merge(new { src });
            var img = CreateTag(HtmlTag.Img, content, routeValueDictionary);
            return img;
        }

        public IHtmlContent Anchor(string href, string content, object attributes = null)
        {
            return Anchor(href, new HtmlString(content), attributes);
        }

        public IHtmlContent Anchor(string href, HtmlString content, object attributes = null)
        {
            var routeValue = AnonymousHelper.ToDictionary(attributes);
            routeValue.Set("href", href);
            var a = CreateTag(HtmlTag.A, content, routeValue);
            return a;
        }

        public IHtmlContent Div(HtmlString content, object attributes = null)
        {
            var div = CreateTag(HtmlTag.Div, content, AnonymousHelper.ToDictionary(attributes));
            return div;
        }

        public IHtmlContent Div(string content, object attributes = null)
        {
            return Div(new HtmlString(content), attributes);
        }

        public IHtmlContent Span(HtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Span, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent Span(string content, object attributes = null)
        {
            return Span(new HtmlString(content), attributes);
        }

        public IHtmlContent I(object attributes = null)
        {
            var tag = CreateTag(HtmlTag.I, HtmlString.Empty, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent P(HtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.P, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent P(string content, object attributes = null)
        {
            return P(new HtmlString(content), attributes);
        }

        public IHtmlContent Ul(HtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Ul, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent Ul(string content, object attributes = null)
        {
            return Ul(new HtmlString(content), attributes);
        }

        public IHtmlContent Li(HtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Li, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent Li(string content, object attributes = null)
        {
            return Li(new HtmlString(content), attributes);
        }

        public IHtmlContent Tag(HtmlTag tag, HtmlString content, object attributes = null)
        {
            var res = CreateTag(tag, content, AnonymousHelper.ToDictionary(attributes));
            return res;
        }

        public BeginTag BeginTag(HtmlTag tag, object attributes = null)
        {
            return new BeginTag(htmlHelper, tag, AnonymousHelper.ToDictionary(attributes));
        }

        public IHtmlContent Tag(HtmlTag tag, string content, object attributes = null)
        {
            return Tag(tag, new HtmlString(content), attributes);
        }

        public IHtmlContent File(string value, object attributes = null)
        {
            var file = CreateInput(value, HtmlInputType.File.ToStringLower(), attributes);
            file.TagRenderMode = TagRenderMode.SelfClosing;
            return file;
        }

        public IHtmlContent RenderDropDownTemplate()
        {
            var templateFactory = IoCFactory.Instance.TryResolve<ITemplateFactory>() ?? new TemplateHandlebarsFactory();
            string template = templateFactory.GetDropDownTemplate();
            return CreateScript("incodingDropDownTemplate", HtmlType.TextTemplate, string.Empty, new HtmlString(template));
        }

        #endregion
    }

}