﻿using System;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Incoding.Core.Maybe;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Options;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Primitive;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Extensions
{
    #region << Using >>

    #endregion

    public static class RouteValueDictionaryExtensions
    {
        public static MvcForm ToMvcForm(this HtmlRouteValueDictionary htmlAttributes, string url,
                                           FormMethod method,
                                           Enctype enctype = Enctype.ApplicationXwwwFormUrlEncoded
                )
        {
            //if (!htmlAttributes.ContainsKey(HtmlAttribute.Method.ToStringLower()))
            //    htmlAttributes.Set(HtmlAttribute.Method.ToStringLower(), EnumExtensions.ToStringLower(method));

            //if (!htmlAttributes.ContainsKey(HtmlAttribute.Enctype.ToStringLower()))
            //    htmlAttributes.Set(HtmlAttribute.Enctype.ToStringLower(), enctype.ToLocalization());

            //if (!htmlAttributes.ContainsKey(HtmlAttribute.Action.ToStringLower()))
            //    htmlAttributes.Set(HtmlAttribute.Action.ToStringLower(), url);

            return new IncodingHtmlHelper(htmlAttributes.HtmlHelper).BeginMvcForm(url, enctype, method, false, htmlAttributes);
        }
        
        public static BeginTag ToBeginForm(this HtmlRouteValueDictionary htmlAttributes, string url,
                                           JqueryAjaxOptions.HttpVerbs method = JqueryAjaxOptions.HttpVerbs.Post,
                                           Enctype enctype = Enctype.ApplicationXwwwFormUrlEncoded
                )
        {
            if (!htmlAttributes.ContainsKey(HtmlAttribute.Method.ToStringLower()))
                htmlAttributes.Set(HtmlAttribute.Method.ToStringLower(), method.ToStringLower());

            if (!htmlAttributes.ContainsKey(HtmlAttribute.Enctype.ToStringLower()))
                htmlAttributes.Set(HtmlAttribute.Enctype.ToStringLower(), enctype.ToLocalization());

            if (!htmlAttributes.ContainsKey(HtmlAttribute.Action.ToStringLower()))
                htmlAttributes.Set(HtmlAttribute.Action.ToStringLower(), url);

            return ToBeginTag(htmlAttributes, HtmlTag.Form);
        }
        
        public static BeginTag ToBeginTag(this HtmlRouteValueDictionary htmlAttributes, HtmlTag tag)
        {
            return new BeginTag(htmlAttributes.HtmlHelper, tag, htmlAttributes);
        }
        
        ////ncrunch: no coverage end

        public static IHtmlContent ToButton(this RouteValueDictionary htmlAttributes)
        {
            return ToButton(htmlAttributes, string.Empty);
        }

        public static IHtmlContent ToButton(this RouteValueDictionary htmlAttributes, string value)
        {
            return htmlAttributes.ToButton(new HtmlString(value));
        }

        public static IHtmlContent ToButton(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToButton(Selector.FromHelperResult(text).ToString());
        }

        public static IHtmlContent ToButton(this RouteValueDictionary htmlAttributes, HtmlString value)
        {
            var tagBuilder = new TagBuilder(HtmlTag.Button.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            tagBuilder.InnerHtml.AppendHtml(value);
            return tagBuilder;
        }

        public static IHtmlContent ToCheckBox(this RouteValueDictionary htmlAttributes, bool value)
        {
            if (value)
                htmlAttributes.Set(HtmlAttribute.Checked.ToStringLower(), "checked");

            return ToInput(htmlAttributes, HtmlInputType.CheckBox, string.Empty);
        }

        public static IHtmlContent ToRadioButton(this RouteValueDictionary htmlAttributes, string value, bool isChecked)
        {
            htmlAttributes.Set(HtmlAttribute.Value.ToStringLower(), value);
            if (isChecked)
                htmlAttributes.Set(HtmlAttribute.Checked.ToStringLower(), "checked");

            return ToInput(htmlAttributes, HtmlInputType.Radio, string.Empty);
        }

        public static IHtmlContent ToDiv(this RouteValueDictionary htmlAttributes)
        {
            return ToTag(htmlAttributes, HtmlTag.Div);
        }

        public static IHtmlContent ToDiv(this RouteValueDictionary htmlAttributes, string content)
        {
            return ToTag(htmlAttributes, HtmlTag.Div, content);
        }

        public static IHtmlContent ToDiv(this RouteValueDictionary htmlAttributes, HtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.Div, content);
        }

        public static IHtmlContent ToDiv(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> content)
        {
            return htmlAttributes.ToTag(HtmlTag.Div, content);
        }

        public static IHtmlContent ToI(this RouteValueDictionary htmlAttributes)
        {
            return ToI(htmlAttributes, string.Empty);
        }

        public static IHtmlContent ToI(this RouteValueDictionary htmlAttributes, HtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.I, content);
        }

        public static IHtmlContent ToI(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToI(new HtmlString(content));
        }

        public static IHtmlContent ToI(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToI(Selector.FromHelperResult(text).ToString());
        }

        public static IHtmlContent ToImg(this RouteValueDictionary htmlAttributes)
        {
            return ToImg(htmlAttributes, string.Empty);
        }

        public static IHtmlContent ToImg(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToImg(new HtmlString(content));
        }

        public static IHtmlContent ToImg(this RouteValueDictionary htmlAttributes, HtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.Img, content);
        }

        public static IHtmlContent ToImg(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToImg(Selector.FromHelperResult(text).ToString());
        }

        public static IHtmlContent ToInput(this RouteValueDictionary htmlAttributes, HtmlInputType inputType, string value)
        {
            var input = new TagBuilder(HtmlTag.Input.ToStringLower());
            input.MergeAttribute(HtmlAttribute.Type.ToLocalization().ToLower(), inputType.ToStringLower());
            if (!string.IsNullOrWhiteSpace(value))
                input.MergeAttribute(HtmlAttribute.Value.ToStringLower(), value);

            input.MergeAttributes(htmlAttributes, true);
            input.TagRenderMode = TagRenderMode.SelfClosing;
            return input;
        }

        public static IHtmlContent ToLabel(this RouteValueDictionary htmlAttributes)
        {
            return ToLabel(htmlAttributes, string.Empty);
        }

        public static IHtmlContent ToLabel(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToLabel(new HtmlString(content));
        }

        public static IHtmlContent ToLabel(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToLabel(Selector.FromHelperResult(text).ToString());
        }

        public static IHtmlContent ToLabel(this RouteValueDictionary htmlAttributes, HtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.Label, content);
        }

        public static IHtmlContent ToLink(this RouteValueDictionary htmlAttributes)
        {
            return ToLink(htmlAttributes, string.Empty);
        }

        public static IHtmlContent ToLink(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToLink(new HtmlString(content));
        }

        public static IHtmlContent ToLink(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToLink(Selector.FromHelperResult(text).ToString());
        }

        public static IHtmlContent ToLink(this RouteValueDictionary htmlAttributes, HtmlString content)
        {
            var tagBuilder = new TagBuilder(HtmlTag.A.ToStringLower());
            tagBuilder.InnerHtml.AppendHtml(content);
            tagBuilder.MergeAttribute(HtmlAttribute.Href.ToStringLower(), "javascript:void(0);", false);
            tagBuilder.MergeAttributes(htmlAttributes, true);
            return tagBuilder;
        }

        public static IHtmlContent ToSelect(this RouteValueDictionary htmlAttributes)
        {
            var select = new TagBuilder(HtmlTag.Select.ToStringLower());
            select.MergeAttributes(htmlAttributes, true);
            return select;
        }

        public static IHtmlContent ToSubmit(this RouteValueDictionary htmlAttributes, string value)
        {
            return ToInput(htmlAttributes, HtmlInputType.Submit, value);
        }

        public static IHtmlContent ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag)
        {
            return ToTag(htmlAttributes, tag, string.Empty);
        }

        public static IHtmlContent ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, string content)
        {
            return htmlAttributes.ToTag(tag, new HtmlString(content));
        }

        public static IHtmlContent ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, Func<object, HelperResult> content)
        {
            return htmlAttributes.ToTag(tag, Selector.FromHelperResult(content).ToString());
        }

        public static IHtmlContent ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, HtmlString content)
        {
            bool isContent = !string.IsNullOrWhiteSpace(content.With(r => r.ToString()));
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            if (isContent)
                tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public static IHtmlContent ToTextArea(this RouteValueDictionary htmlAttributes)
        {
            return ToTag(htmlAttributes, HtmlTag.TextArea);
        }

        public static IHtmlContent ToTextBox(this RouteValueDictionary htmlAttributes, string value = "")
        {
            return ToInput(htmlAttributes, HtmlInputType.Text, value);
        }
    }
}