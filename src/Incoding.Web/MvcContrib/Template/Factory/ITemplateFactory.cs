using Incoding.Mvc.MvcContrib.Template.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Template.Factory
{
    public interface ITemplateFactory
    {        
        ITemplateSyntax<TModel> ForEach<TModel>(IHtmlHelper htmlHelper);

        ITemplateSyntax<TModel> NotEach<TModel>(IHtmlHelper htmlHelper);

        string GetDropDownTemplate();

        string Render<T>(IHtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class;
    }
}