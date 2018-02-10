using System;
using Incoding.Mvc.MvcContrib.Template.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Template.Factory
{
    #region << Using >>

    #endregion

    public class TemplateDoTFactory : ITemplateFactory
    {
        #region ITemplateFactory Members

        public ITemplateSyntax<TModel> ForEach<TModel>(IHtmlHelper htmlHelper)
        {
            return new TemplateDoTSyntax<TModel>(htmlHelper, "data");
        }

        public ITemplateSyntax<TModel> NotEach<TModel>(IHtmlHelper htmlHelper)
        {
            return new TemplateDoTSyntax<TModel>(htmlHelper, "data");
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
            throw new NotImplementedException();
        }

        #endregion
    }
}