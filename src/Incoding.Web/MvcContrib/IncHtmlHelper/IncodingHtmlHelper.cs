using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Incoding.Core.Block.IoC;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Web.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib
{
    public class IncodingHtmlHelper
    {
        private readonly IHtmlHelper htmlHelper;
        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global

        ////ncrunch: no coverage start

        #region Static Fields

        public static Selector DropDownTemplateId = "incodingDropDownTemplate".ToId();

        public static JqueryAjaxOptions DropDownOption = new JqueryAjaxOptions(JqueryAjaxOptions.Default);

        public static BootstrapOfVersion BootstrapVersion = BootstrapOfVersion.v2;

        public static B Def_Label_Class = B.Col_xs_5;

        public static B Def_Control_Class = B.Col_xs_7;

        internal IncodingHtmlHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        internal static TagBuilder CreateScript(string id, HtmlType type, string src, IHtmlContent content)
        {
            var routeValueDictionary = new RouteValueDictionary(new { type = type.ToLocalization() });
            if (!string.IsNullOrWhiteSpace(src))
                routeValueDictionary.Merge(new { src });
            if (!string.IsNullOrWhiteSpace(id))
                routeValueDictionary.Merge(new { id });

            return CreateTag(HtmlTag.Script, content, routeValueDictionary);
        }

        internal static TagBuilder CreateInput(string value, string type, object attributes)
        {
            var routeValueDictionary = AnonymousHelper.ToDictionary(attributes);
            routeValueDictionary.Merge(new { value, type });
            var input = CreateTag(HtmlTag.Input, HtmlString.Empty, routeValueDictionary);

            return input;
        }

        internal static TagBuilder CreateTag(HtmlTag tag, IHtmlContent content, RouteValueDictionary attributes)
        {
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.InnerHtml.AppendHtml(content.ReturnOrDefault(r => r, HtmlString.Empty));
            tagBuilder.MergeAttributes(attributes, true);

            return tagBuilder;
        }

        public MvcForm BeginMvcForm(string url, Enctype enctype, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            this.htmlHelper.ViewContext.FormContext = new FormContext()
            {
                CanRenderAtEndOfForm = true
            };
            return this.GenerateMvcForm(url, method, enctype, antiforgery, htmlAttributes);
        }

        private static IDictionary<string, object> GetHtmlAttributeDictionaryOrNull(object htmlAttributes)
        {
            IDictionary<string, object> dictionary = (IDictionary<string, object>)null;
            if (htmlAttributes != null)
                dictionary = htmlAttributes as IDictionary<string, object> ?? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return dictionary;
        }

        protected virtual TagBuilder GenerateFormCore(ViewContext viewContext, string action, Enctype enctype, string method, object htmlAttributes)
        {
            if (viewContext == null)
                throw new ArgumentNullException(nameof(viewContext));
            TagBuilder tagBuilder = new TagBuilder("form");
            IDictionary<string, object> dictionaryOrNull = GetHtmlAttributeDictionaryOrNull(htmlAttributes);
            tagBuilder.MergeAttributes<string, object>(dictionaryOrNull);
            string actionKey = nameof(action);
            tagBuilder.MergeAttribute(actionKey, action);
            if (string.IsNullOrEmpty(method))
                method = "post";
            string methodName = nameof(method);
            tagBuilder.MergeAttribute(methodName, method, true);
            string enctypeName = nameof(enctype);
            tagBuilder.MergeAttribute(enctypeName, enctype.ToLocalization(), true);
            return tagBuilder;
        }

        protected virtual MvcForm GenerateMvcForm(string url, FormMethod method, Enctype enctype, bool? antiforgery, object htmlAttributes)
        {
            TagBuilder form = this.GenerateFormCore(this.htmlHelper.ViewContext, url, enctype, HtmlHelper.GetFormMethodString(method), htmlAttributes);
            if (form != null)
            {
                form.TagRenderMode = TagRenderMode.StartTag;
                form.WriteTo(this.htmlHelper.ViewContext.Writer, HtmlEncoder.Default);
            }
            if ((antiforgery.HasValue ? (antiforgery.Value ? 1 : 0) : ((uint)method > 0U ? 1 : 0)) != 0)
                this.htmlHelper.ViewContext.FormContext.EndOfFormContent.Add(this.htmlHelper.AntiForgeryToken());

            return new MvcForm(htmlHelper.ViewContext, HtmlEncoder.Default);
        }

        public MvcTemplate<TModel> Template<TModel>()
        {
            return new MvcTemplate<TModel>(this.htmlHelper);
        }

        public IHtmlContent Script([PathReference] string src)
        {
            var script = IncodingHtmlHelper.CreateScript(string.Empty, HtmlType.TextJavaScript, src, HtmlString.Empty);
            return script;
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

        public IHtmlContent Img(string src, IHtmlContent content, object attributes = null)
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

        public IHtmlContent Anchor(string href, IHtmlContent content, object attributes = null)
        {
            var routeValue = AnonymousHelper.ToDictionary(attributes);
            routeValue.Set("href", href);
            var a = CreateTag(HtmlTag.A, content, routeValue);
            return a;
        }

        public IHtmlContent Div(IHtmlContent content, object attributes = null)
        {
            var div = CreateTag(HtmlTag.Div, content, AnonymousHelper.ToDictionary(attributes));
            return div;
        }

        public IHtmlContent Div(string content, object attributes = null)
        {
            return Div(new HtmlString(content), attributes);
        }

        public IHtmlContent Span(IHtmlContent content, object attributes = null)
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

        public IHtmlContent P(IHtmlContent content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.P, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent P(string content, object attributes = null)
        {
            return P(new HtmlString(content), attributes);
        }

        public IHtmlContent Ul(IHtmlContent content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Ul, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent Ul(string content, object attributes = null)
        {
            return Ul(new HtmlString(content), attributes);
        }

        public IHtmlContent Li(IHtmlContent content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Li, content, AnonymousHelper.ToDictionary(attributes));
            return tag;
        }

        public IHtmlContent Li(string content, object attributes = null)
        {
            return Li(new HtmlString(content), attributes);
        }

        public IHtmlContent Tag(HtmlTag tag, IHtmlContent content, object attributes = null)
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

        ////ncrunch: no coverage end

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
    }

    public class IncodingHtmlHelper<TModel> : IncodingHtmlHelper where TModel : new()
    {
        readonly IHtmlHelper<TModel> htmlHelper;

        public IncodingHtmlHelper(IHtmlHelper<TModel> htmlHelper) : base(htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public MvcForm BeginPush(Action<BeginPushSetting> evaluated)
        {
            var setting = new BeginPushSetting();
            evaluated(setting);

            return BeginPushInternal(setting);
        }

        private MvcForm BeginPushInternal(BeginPushSetting setting)
        {
            var routes = new RouteValueDictionary(new { @class = setting.CssClass, id = setting.Id ?? "" });
            
            return htmlHelper.When(JqueryBind.InitIncoding)
                .OnSuccess(dsl => dsl.Self().Form.Validation.Parse())
                .When(JqueryBind.Submit)
                .PreventDefault().StopPropagation()
                .Submit()
                .OnBegin(dsl =>
                {
                    setting.OnBegin?.Invoke(dsl);
                })
                .OnSuccess(dsl =>
                {
                    setting.OnSuccess?.Invoke(dsl);
                })
                .OnComplete(dsl =>
                {
                    setting.OnComplete?.Invoke(dsl);
                })
                .OnError(dsl =>
                {
                    dsl.Self().Form.Validation.Refresh();
                    setting.OnError?.Invoke(dsl);
                })

                .AsHtmlAttributes(routes)
                .ToMvcForm(setting.Url ?? new UrlHelper(this.htmlHelper.ViewContext).Dispatcher().Push<TModel>(), setting.Method, setting.EncType);
        }

        

        public class BeginPushSetting
        {
            public BeginPushSetting()
            {
                CssClass = "form-horizontal";
            }

            public Action<IIncodingMetaLanguageCallbackBodyDsl> OnSuccess { get; set; }

            public Action<IIncodingMetaLanguageCallbackBodyDsl> OnBegin { get; set; }

            public Action<IIncodingMetaLanguageCallbackBodyDsl> OnComplete { get; set; }

            public Action<IIncodingMetaLanguageCallbackBodyDsl> OnError { get; set; }

            public string CssClass { get; set; }
            
            public string Id { get; set; }

            public string Url { get; set; }
            public FormMethod Method { get; set; }
            public Enctype EncType { get; set; }
        }
    
    }

}