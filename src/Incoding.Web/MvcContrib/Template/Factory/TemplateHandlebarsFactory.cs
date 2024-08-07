﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using HandlebarsDotNet;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class TemplateHandlebarsFactory : ITemplateFactory
    {
//#if netcoreapp2_1
//        internal static readonly ConcurrentDictionary<string, Func<object, string>> cache = new ConcurrentDictionary<string, Func<object, string>>();
//#else
        internal static readonly ConcurrentDictionary<string, HandlebarsTemplate<object, object>> cache = new ConcurrentDictionary<string, HandlebarsTemplate<object, object>>();
//#endif
        public static Func<string> GetVersion = () => { return string.Empty; };

#region ITemplateFactory Members

        public ITemplateSyntax<TModel> ForEach<TModel>(IHtmlHelper htmlHelper)
        {
            return new TemplateHandlebarsSyntax<TModel>(htmlHelper, "data", HandlebarsType.Each, string.Empty);
        }

        public ITemplateSyntax<TModel> NotEach<TModel>(IHtmlHelper htmlHelper)
        {
            return new TemplateHandlebarsSyntax<TModel>(htmlHelper, "data", HandlebarsType.Unless, string.Empty);
        }

        public string GetDropDownTemplate()
        {
            return @"{{#data}}
                                 {{#if Title}}
                                 <optgroup label=""{{Title}}"">
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 </optgroup>
                                 {{else}}
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 {{/if}}
                                 {{/data}}";
        }

        public string Render<T>(IHtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);
            object correctData = data;
            if (data != null && !data.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
                correctData = new { data = data };

            return cache.GetOrAdd(fullPathToView + GetVersion(), (i) =>
                                                                 {
                                                                     //var viewRenderService = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IViewRenderService>();
                                                                     var tmpl = htmlHelper.PartialAsync(pathToView, modelForView).Result.HtmlContentToString();
                                                                     return Handlebars.Compile(tmpl);
                                                                 })(new { data = correctData });
        }

#endregion
    }
}