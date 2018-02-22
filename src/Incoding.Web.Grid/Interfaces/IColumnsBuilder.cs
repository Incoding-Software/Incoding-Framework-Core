using System;
using System.Linq.Expressions;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Web.Grid.Interfaces
{
    public interface IColumnsBuilder<T> where T : class
    {
        IColumn<T> Bound(Expression<Func<T, object>> argExpression);
        IColumn<T> Template(Func<ITemplateSyntax<T>, HelperResult> template);
    }
}