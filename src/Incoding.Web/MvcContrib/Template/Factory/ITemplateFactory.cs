using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    public interface ITemplateFactory
    {        
        ITemplateSyntax<TModel> ForEach<TModel>(IHtmlHelper htmlHelper);

        ITemplateSyntax<TModel> NotEach<TModel>(IHtmlHelper htmlHelper);

        string GetDropDownTemplate();

        string Render<T>(IHtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class;
    }
}