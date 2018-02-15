﻿using System;
using System.Collections;
using Incoding.Core.Extensions;
using Incoding.Core.Maybe;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Jquery;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.DSL.Instances.Jquery
{
    #region << Using >>

    #endregion

    public class IncodingMetaCallbackJqueryAttributesDsl
    {
        #region Fields

        private readonly IHtmlHelper _htmlHelper;
        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryAttributesDsl(IHtmlHelper htmlHelper, IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            _htmlHelper = htmlHelper;
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Add or remove attribute
        /// </summary>
        public IExecutableSetting Toggle(HtmlAttribute attribute)
        {
            return Toggle(attribute.ToJqueryString());
        }

        /// <summary>
        ///     Add or remove attribute
        /// </summary>
        public IExecutableSetting Toggle(string attribute)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("toggleProp", attribute);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive.HtmlAttribute.Checked" />
        /// </summary>
        public IExecutableSetting ToggleChecked()
        {
            return Toggle(HtmlAttribute.Checked);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive.HtmlAttribute.Disabled" />
        /// </summary>
        public IExecutableSetting ToggleDisabled()
        {
            return Toggle(HtmlAttribute.Disabled);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive.HtmlAttribute.Readonly" />
        /// </summary>
        public IExecutableSetting ToggleReadonly()
        {
            return Toggle(HtmlAttribute.Readonly);
        }

        /// <summary>
        ///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class's
        ///     presence or the value of the switch argument.
        /// </summary>
        /// <param name="class">
        ///     One or more <c>class</c> names (separated by spaces) to be toggled for each element in the matched set.
        /// </param>
        public IExecutableSetting ToggleClass(string @class)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("toggleClass", @class);
        }

        /// <summary>
        ///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class's
        ///     presence or the value of the switch argument.
        /// </summary>
        /// <param name="class">
        ///     Bootstrap classes
        /// </param>
        public IExecutableSetting ToggleClass(B @class)
        {
            return ToggleClass(@class.ToJqueryString());
        }

        /// <summary>
        ///     <c>Remove</c> a single <c>class</c>, multiple classes, or all classes from each element in the set of matched
        ///     elements.
        /// </summary>
        /// <param name="class">
        ///     One or more space-separated classes to be removed from the class attribute of each matched
        ///     element.
        /// </param>
        public IExecutableSetting RemoveClass(string @class)
        {
            return this.plugInDsl.Registry(new ExecutableJquery(ExecutableJquery.Method.RemoveClass, new[] { @class }));            
        }

        /// <summary>
        ///     <c>Remove</c> a single <c>class</c>, multiple classes, or all classes from each element in the set of matched
        ///     elements.
        /// </summary>
        /// <param name="class">
        ///     Bootstrap classes
        /// </param>
        public IExecutableSetting RemoveClass(B @class)
        {
            return RemoveClass(@class.ToJqueryString());
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        [Obsolete("Please use Remove")]
        public IExecutableSetting RemoveAttr(string key)
        {
            return Remove(key);
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting Remove(string key)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("removeAttr", key.ToLower());
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting Remove(HtmlAttribute key)
        {
            return Remove(key.ToJqueryString());
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        [Obsolete("Please use Remove")]
        public IExecutableSetting RemoveAttr(HtmlAttribute key)
        {
            return Remove(key);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        [Obsolete("Please use Set")]
        public IExecutableSetting SetAttr(string key, Selector value = null)
        {
            return Set(key, value);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting Set(string key, Selector value = null)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("attr", key, value ?? key);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        [Obsolete("Please use Set")]
        public IExecutableSetting SetAttr(HtmlAttribute key, Selector value = null)
        {
            return Set(key, value);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting Set(HtmlAttribute key, Selector value = null)
        {
            return Set(key.ToJqueryString(), value);
        }

        /// <summary>
        ///     Remove a property for the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveProp(string key)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("removeProp", key);
        }

        /// <summary>
        ///     Remove a property for the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveProp(HtmlAttribute key)
        {
            return RemoveProp(key.ToJqueryString());
        }

        /// <summary>
        ///     Set one properties for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting SetProp(string key, Selector value = null)
        {
            return plugInDsl.Core(_htmlHelper).JQuery.Call("prop", key, value ?? key);
        }

        /// <summary>
        ///     Set one properties for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Selectors.Core.Selector" />
        /// </param>
        public IExecutableSetting SetProp(HtmlAttribute key, Selector value = null)
        {
            return SetProp(key.ToJqueryString(), value);
        }

        /// <summary>
        ///     Set the <paramref name="value" /> of each element in the set of matched elements.
        /// </summary>
        /// <param name="value">
        ///     A string of text or an array of strings corresponding to the value of each matched element to set
        ///     as selected
        /// </param>
        public IExecutableSetting Val(object value)
        {
            if (value is IEnumerable)
                return plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Val.F(value.ToJsonString())));

            return plugInDsl.Core(_htmlHelper).JQuery.Call("val", value.Recovery(string.Empty));
        }

        /// <summary>
        ///     Set the <paramref name="value" /> of each element in the set of matched elements.
        /// </summary>
        /// <param name="value">
        ///     A string of text or an array of strings corresponding to the value of each matched element to set
        ///     as selected
        /// </param>
        public IExecutableSetting Val(Func<JquerySelector, JquerySelectorExtend> value)
        {
            if (value == null)
                return Val((object)null);

            return plugInDsl.Core(_htmlHelper).JQuery.Call("val", value(Selector.Jquery));
        }

        /// <summary>
        ///     Adds the specified <c>class</c> to each of the set of matched elements
        /// </summary>
        /// <param name="class">
        ///     One or more <c>class</c> names to be added to the class attribute of each matched element.
        /// </param>
        [ContractAnnotation("class:null =>true")]
        public IExecutableSetting AddClass([HtmlAttributeValue("class")] string @class)
        {
            return this.plugInDsl.Registry(new ExecutableJquery(ExecutableJquery.Method.AddClass, new[] { @class }));
        }


        /// <summary>
        ///     Adds the specified <c>class</c> to each of the set of matched elements
        /// </summary>
        /// <param name="class">Twitter bootstrap classes</param>
        public IExecutableSetting AddClass(B @class)
        {
            return AddClass(@class.ToJqueryString());
        }

        #endregion
    }
}