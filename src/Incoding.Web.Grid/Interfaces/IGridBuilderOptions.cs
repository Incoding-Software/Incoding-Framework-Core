﻿using System;
using Incoding.Web.Grid.Components;
using Incoding.Web.Grid.GridParts;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.Grid.Interfaces
{
    public interface IGridBuilderOptions<T> where T : class
    {
        IGridBuilderOptions<T> Styling(string @class);
        IGridBuilderOptions<T> Styling(BootstrapTable @class);
        IGridBuilderOptions<T> Columns(Action<ColumnsBuilder<T>> builderAction);
        IGridBuilderOptions<T> NextRowContent(Func<ITemplateSyntax<T>, HelperResult> content);
        IGridBuilderOptions<T> NextRow(Action<Row<T>> action);
        IGridBuilderOptions<T> Scrolling(int height);
        IGridBuilderOptions<T> RowAttr(RouteValueDictionary htmlAttributes);
        IGridBuilderOptions<T> RowAttr(object htmlAttributes);
        IGridBuilderOptions<T> RowClass(Func<ITemplateSyntax<T>, HelperResult> template);
        IGridBuilderOptions<T> SetTBodyAttrValue(string key, Func<ITemplateSyntax<T>, HelperResult> tbodyMeta);
        IGridBuilderOptions<T> RowClass(B htmlClass);
        IGridBuilderOptions<T> RowClass(string htmlClass);
        IGridBuilderOptions<T> HeadAttr(RouteValueDictionary htmlAttributes);
        IGridBuilderOptions<T> HeadAttr(object htmlAttributes);
        IGridBuilderOptions<T> HeadClass(B htmlClass);
        IGridBuilderOptions<T> HeadClass(string htmlClass);
        IGridBuilderOptions<T> AjaxGet(string actionString);
        IGridBuilderOptions<T> Sortable();

        [Obsolete("Please use OnSuccess")]
        IGridBuilderOptions<T> OnBind(Action<IIncodingMetaLanguageCallbackBodyDsl> action);
        IGridBuilderOptions<T> OnSuccess(Action<IIncodingMetaLanguageCallbackBodyDsl> action);
        IGridBuilderOptions<T> OnBegin(Action<IIncodingMetaLanguageCallbackBodyDsl> action);
        IGridBuilderOptions<T> NoRecords(Selector noRecordsSelector);
        IGridBuilderOptions<T> Pageable();
        IGridBuilderOptions<T> Pageable(string templateView);
        IGridBuilderOptions<T> Pageable(Action<PageableBuilder<T>> builderAction);
        IGridBuilderOptions<T> BindEvent(JqueryBind bindEvent);
    }
}