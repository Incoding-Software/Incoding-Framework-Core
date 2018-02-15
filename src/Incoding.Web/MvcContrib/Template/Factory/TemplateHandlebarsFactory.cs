using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Incoding.CQRS;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language;
using Incoding.Mvc.MvcContrib.Template.Syntax;
using Incoding.Web.MvcContrib.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Mvc.MvcContrib.Template.Factory
{
    #region << Using >>

    #endregion

    public class TemplateHandlebarsFactory : ITemplateFactory
    {
        internal static readonly ConcurrentDictionary<string, Func<object, string>> cache = new ConcurrentDictionary<string, Func<object, string>>();

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
                                                                     var viewRenderService = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IViewRenderService>();
                                                                     var tmpl = viewRenderService
                                                                        .RenderToStringAsync(htmlHelper.ViewContext, pathToView, modelForView).Result;
                                                                     return Handlebars.Compile(tmpl);
                                                                 })(new { data = correctData });
        }

        #endregion
    }
}