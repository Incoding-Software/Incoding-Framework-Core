using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using Incoding.Core;
using Incoding.Core.Extensions;
using Incoding.Web.Extensions;
using Incoding.Web.Grid.Attributes;
using Incoding.Web.Grid.Components;
using Incoding.Web.Grid.GridParts;
using Incoding.Web.Grid.HtmlHelpers;
using Incoding.Web.Grid.Interfaces;
using Incoding.Web.Grid.Options;
using Incoding.Web.Grid.Paging;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.Grid
{
    public class GridBuilder<T> : IGridBuilder<T>, IGridBuilderOptions<T>, IHtmlContent where T : class
    {
        #region Constructors

        public GridBuilder(IHtmlHelper htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        #endregion

        #region Fields

        readonly IHtmlHelper _htmlHelper;

        string _ajaxGetAction, _gridClass, _templateId, _contentTable, _pagingTemplateId, _sortBySelector, _descSelector, _pagingContainer, _pageSizesSelect;

        Selector _noRecordsSelector;

        bool _isPageable, _isScrolling, _showItemsCount, _showPageSizes;

        int[] _pageSizesArray;

        int _contentTableHeight;

        IDictionary<string, object> RowAttributes, HeaderRowAttributes;

        Dictionary<string, Func<ITemplateSyntax<T>, HelperResult>> TBodyAttrs = new Dictionary<string, Func<ITemplateSyntax<T>, HelperResult>>();

        Func<ITemplateSyntax<T>, HelperResult> RowTemplateClass;

        readonly List<Column<T>> _сolumnList = new List<Column<T>>();

        readonly List<Row<T>> _nextRowList = new List<Row<T>>();

        string _customPagingTemplateView;

        Action<IIncodingMetaLanguageCallbackBodyDsl> _onSuccess = dsl => { };
        Action<IIncodingMetaLanguageCallbackBodyDsl> _onBegin = dsl => { };

        JqueryBind _bindEvent = JqueryBind.InitIncoding;

        #endregion

        #region Api Methods

        public IGridBuilderOptions<T> Id(string gridId)
        {
            Func<string, string> SetName = (part) => "{0}-{1}".F(gridId, part);

            this._contentTable = gridId;
            this._templateId = SetName("template");
            this._pagingTemplateId = SetName("pagingTemplateId");
            this._sortBySelector = SetName("sortBySelector");
            this._descSelector = SetName("descSelector");
            this._pagingContainer = SetName("pagingContainer");
            this._pageSizesSelect = SetName("pageSizesSelect");

            return this;
        }

        public IGridBuilderOptions<T> Styling(string @class)
        {
            this._gridClass = @class;
            return this;
        }

        public IGridBuilderOptions<T> Styling(BootstrapTable @class)
        {
            var classes = Enum.GetValues(typeof(BootstrapTable))
                .Cast<BootstrapTable>()
                .Where(r => @class.HasFlag(r))
                .Select(r => r.ToLocalization())
                .AsString(" ");
            return Styling(classes);
        }

        public IGridBuilderOptions<T> Columns(Action<ColumnsBuilder<T>> builderAction)
        {
            builderAction(new ColumnsBuilder<T>(this));
            return this;
        }

        public IGridBuilderOptions<T> NextRowContent(Func<ITemplateSyntax<T>, HelperResult> content)
        {
            this._nextRowList.Add(new Row<T>(content));
            return this;
        }

        public IGridBuilderOptions<T> NextRow(Action<Row<T>> action)
        {
            Row<T> row = new Row<T>();
            action(row);
            this._nextRowList.Add(row);
            return this;
        }

        public IGridBuilderOptions<T> Scrolling(int height)
        {
            this._isScrolling = true;
            this._contentTableHeight = height;
            return this;
        }

        //for tbody tr

        public IGridBuilderOptions<T> RowAttr(RouteValueDictionary htmlAttributes)
        {
            this.RowAttributes = htmlAttributes;
            return this;
        }

        public IGridBuilderOptions<T> RowAttr(object htmlAttributes)
        {
            return RowAttr(AnonymousHelper.ToDictionary(htmlAttributes));
        }

        public IGridBuilderOptions<T> RowClass(Func<ITemplateSyntax<T>, HelperResult> template)
        {
            this.RowTemplateClass = template;
            return this;
        }


        public IGridBuilderOptions<T> RowClass(B htmlClass)
        {
            return RowAttr(AnonymousHelper.ToDictionary(new { @class = htmlClass.ToLocalization() }));
        }

        public IGridBuilderOptions<T> RowClass(string htmlClass)
        {
            return RowAttr(AnonymousHelper.ToDictionary(new { @class = htmlClass }));
        }

        //end for tbody tr

        public IGridBuilderOptions<T> SetTBodyAttrValue(string key, Func<ITemplateSyntax<T>, HelperResult> keyValue)
        {
            this.TBodyAttrs.Add(key, keyValue);
            return this;
        }

        //for thead tr
        public IGridBuilderOptions<T> HeadAttr(RouteValueDictionary htmlAttributes)
        {
            this.HeaderRowAttributes = htmlAttributes;
            return this;
        }

        public IGridBuilderOptions<T> HeadAttr(object htmlAttributes)
        {
            return HeadAttr(AnonymousHelper.ToDictionary(htmlAttributes));
        }

        public IGridBuilderOptions<T> HeadClass(B htmlClass)
        {
            return HeadAttr(AnonymousHelper.ToDictionary(new { @class = htmlClass.ToLocalization() }));
        }

        public IGridBuilderOptions<T> HeadClass(string htmlClass)
        {
            return HeadAttr(AnonymousHelper.ToDictionary(new { @class = htmlClass }));
        }

        //end for thead tr

        public IGridBuilderOptions<T> AjaxGet(string actionString)
        {
            this._ajaxGetAction = actionString.AppendToQueryString(new
            {
                Desc = Selector.Jquery.Name(this._descSelector),
                SortBy = Selector.Jquery.Name(this._sortBySelector)
            });
            return this;
        }

        public IGridBuilderOptions<T> Sortable()
        {
            if (_сolumnList.Any())
            {
                foreach (var column in _сolumnList)
                    column.SortBy = column.Expression;

                _сolumnList.First().SortDefault = true;
            }
            return this;
        }

        [Obsolete("Please use OnSuccess")]
        public IGridBuilderOptions<T> OnBind(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            this._onSuccess = action;
            return this;
        }

        public IGridBuilderOptions<T> OnSuccess(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            this._onSuccess = action;
            return this;
        }

        public IGridBuilderOptions<T> OnBegin(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            this._onBegin = action;
            return this;
        }

        public IGridBuilderOptions<T> NoRecords(Selector noRecordsSelector)
        {
            this._noRecordsSelector = noRecordsSelector;
            return this;
        }

        public IGridBuilderOptions<T> Pageable()
        {
            this._isPageable = true;
            return this;
        }

        public IGridBuilderOptions<T> Pageable(string templateView)
        {
            this._customPagingTemplateView = templateView;
            return Pageable();
        }

        public IGridBuilderOptions<T> Pageable(Action<PageableBuilder<T>> builderAction)
        {
            builderAction(new PageableBuilder<T>(this));
            return Pageable();
        }

        public IGridBuilderOptions<T> BindEvent(JqueryBind bindEvent)
        {
            this._bindEvent = bindEvent;
            return this;
        }

        #endregion


        #region PrivateMethods

        IHtmlContent Render()
        {
            var divMain = new TagBuilder("div");
            divMain.AddCssClass("inc-grid");

            var table = new TagBuilder("table");
            table.AddCssClass(string.IsNullOrWhiteSpace(this._gridClass) ? GridOptions.Default.GetStyling() : this._gridClass);
            table.AddCssClass("table");


            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            if (!_сolumnList.Any())
                AutoBind();

            foreach (var column in this._сolumnList)
                tr.InnerHtml.AppendHtml(AddTh(column));

            thead.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(thead);
            divMain.InnerHtml.AppendHtml(table);

            GenerateRowTemplate();
            if (_isPageable)
            {
                GeneratePagingTemplate();
                divMain.InnerHtml.AppendHtml(AddPageableTemplate());
            }
            else
            {
                divMain.InnerHtml.AppendHtml(AddTemplate());
            }

            if (this._сolumnList.Any(r => r.SortBy != null))
            {
                var defaultSortColumn = this._сolumnList.FirstOrDefault(r => r.SortDefault);
                divMain.InnerHtml.AppendHtml(this._htmlHelper.Hidden(this._sortBySelector, defaultSortColumn.With(r => r.SortBy.ToString())));
                divMain.InnerHtml.AppendHtml(this._htmlHelper.CheckBox(this._descSelector, defaultSortColumn.With(r => r.IsDescDefault), new { style = "display: none;" }));
            }

            return divMain;
        }

        IHtmlContent AddTh(Column<T> column)
        {
            var attributes = column.ColumnHeaderAttributes ?? column.ColumnAttributes;
            var thWidth = String.IsNullOrWhiteSpace(column.ColumnWidthPct) ? column.ColumnWidth : column.ColumnWidthPct;

            var th = new TagBuilder("th");

            if (column.SortBy != null)
            {
                var link = SortArrow(setting =>
                {
                    setting.Content = column.Name;
                    setting.TargetId = this._contentTable;
                    setting.By = column.SortBy;
                    setting.SortDefault = column.SortDefault;
                    setting.IsDescDefault = column.IsDescDefault;
                });
                th.AddCssClass("col-sortable");
                th.InnerHtml.AppendHtml(link);
            }
            else
                th.InnerHtml.AppendHtml(column.Name);

            //thead tr attributes first
            if (this.HeaderRowAttributes != null)
                th.MergeAttributes(this.HeaderRowAttributes);

            //unique th attributes
            if (attributes != null)
                th.MergeAttributes(attributes, true);

            if (!String.IsNullOrWhiteSpace(thWidth) && (attributes == null || !attributes.ContainsKey("style")))
                th.MergeAttribute("style", "width:{0}{1};".F(thWidth, String.IsNullOrWhiteSpace(column.ColumnWidthPct) ? "px" : "%"));

            if (!column.IsVisible)
                th.AddCssClass("hide");

            return th;
        }

        IHtmlContent AddTemplate()
        {
            var table = this._htmlHelper.When(this._bindEvent)
                            .PreventDefault().StopPropagation()
                            .Ajax(this._ajaxGetAction)
                            .OnBegin(dsl =>
                                     {
                                         this._onBegin?.Invoke(dsl);
                                     })
                            .OnSuccess(dsl =>
                                       {
                                           dsl.Self().Insert.WithTemplateByUrl(GridTemplate.BuildTemplateUrl(_templateId)).Html();

                                           var noRecordContent = this._noRecordsSelector ?? GridOptions.Default.NoRecordsSelector ?? "<caption>No records to display.<caption>";
                                           dsl.Self().JQuery.Dom.Use(noRecordContent).Html().If(() => Selector.Result.IsEmpty());
                                           this._onSuccess?.Invoke(dsl);
                                       })
                            .OnError(dsl => dsl.Self().JQuery.Dom.Use("Error ajax get").Html())
                            .AsHtmlAttributes(new { id = this._contentTable, @class = "table " + (string.IsNullOrWhiteSpace(this._gridClass) ? GridOptions.Default.GetStyling() : this._gridClass) })
                            .ToTag(HtmlTag.Table);

            var divContent = new TagBuilder("div");
            divContent.AddCssClass("content-table");
            if (this._isScrolling)
                divContent.MergeAttribute("style", "height: {0}px; overflow: auto;".F(this._contentTableHeight));
            divContent.InnerHtml.AppendHtml(table);

            return divContent;
            //return new MvcHtmlString("{0}{1}".F(divContent, CreateTemplate()));
        }

        IHtmlContent AddPageableTemplate()
        {
            var tableWithPageable = this._htmlHelper.When(this._bindEvent | JqueryBind.IncChangeUrl)
                                        .StopPropagation().PreventDefault()
                                        .Ajax(this._ajaxGetAction)
                                        .OnBegin(dsl =>
                                          {
                                              if (this._onBegin != null)
                                                  this._onBegin(dsl);
                                          })
                                        .OnSuccess(dsl =>
                                                   {
                                                       dsl.Self().JQuery.Dom.Use(Selector.Result.For<PagingResult<T>>(result => result.Items)).WithTemplateByUrl(GridTemplate.BuildTemplateUrl(_templateId)).Html();

                                                       var insertDsl = dsl.WithId(_pagingContainer).JQuery.Dom.Use(Selector.Result.For<PagingResult<T>>(result => result.Paging));
                                                       if (string.IsNullOrWhiteSpace(_customPagingTemplateView))
                                                           insertDsl.WithTemplateByUrl(
                                                               GridTemplate.BuildTemplateUrl(_pagingTemplateId)).Html();
                                                       else
                                                           insertDsl.WithTemplateByView(_customPagingTemplateView).Html();

                                                       var noRecordContent = this._noRecordsSelector ?? GridOptions.Default.NoRecordsSelector ?? "<caption>No records to display.<caption>";
                                                       dsl.Self().JQuery.Dom.Use(noRecordContent).Html().If(() => Selector.Result.For<PagingResult<T>>(r => r.Items).IsEmpty());

                                                       if (_showItemsCount)
                                                           dsl.WithId(_pagingContainer).JQuery.Dom.Use(Selector.Result.For<PagingResult<T>>(result => result.PagingRange)).Append();

                                                       if (this._onSuccess != null)
                                                           this._onSuccess(dsl);
                                                   })
                                        .OnError(dsl => dsl.Self().JQuery.Dom.Use("Error ajax get").Html())
                                        .AsHtmlAttributes(new { id = this._contentTable, @class = "table " + (string.IsNullOrWhiteSpace(this._gridClass) ? GridOptions.Default.GetStyling() : this._gridClass) })
                                        .ToTag(HtmlTag.Table);

            var divContent = new TagBuilder("div");
            divContent.AddCssClass("content-table");
            divContent.InnerHtml.AppendHtml(tableWithPageable);

            var divPagingContainer = new TagBuilder("div");
            divPagingContainer.GenerateId(_pagingContainer, "-");
            divPagingContainer.AddCssClass("pagination");

            var selectPageSizes = this._htmlHelper.DropDownList("PageSize", new SelectList(_pageSizesArray ?? new int[] { 5, 10, 50, 100 }), null,
                                                                _htmlHelper.When(JqueryBind.Change)
                                                                           .OnSuccess(dsl =>
                                                                                      {
                                                                                          dsl.Self().Store.Hash.Insert();
                                                                                          dsl.Self().Store.Hash.Manipulate(manipulateDsl => manipulateDsl.Set("Page", 1));
                                                                                      })
                                                                           .AsHtmlAttributes(new { style = "width: 50px;", id = _pageSizesSelect }));
            TagBuilder tb = new TagBuilder("div");
            tb.InnerHtml.AppendHtml(divContent);
            tb.InnerHtml.AppendHtml(divPagingContainer);
            if (_showPageSizes)
                tb.InnerHtml.AppendHtml(selectPageSizes);
            return tb;
            //return new MvcHtmlString("{0}{1}{2}{3}".F(divContent,
            //                                          CreateTemplate(),
            //                                          divPagingContainer.ToString(),
            //                                          _showPageSizes ? selectPageSizes.ToString() : ""));
        }

        private void GenerateRowTemplate()
        {
            if (GridTemplate.Templates.ContainsKey(_templateId))
                return;

            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                using (var wrappedHtmlHelper = new HtmlHelperWriterWrapped(_htmlHelper, writer))
                {
                    using (var template = wrappedHtmlHelper.HtmlHelper.Incoding().Template<T>())
                    {
                        using (var each = template.ForEach())
                        {
                            var tbody = new TagBuilder("tbody");
                            foreach (var bodyAttr in this.TBodyAttrs)
                            {
                                tbody.MergeAttribute(bodyAttr.Key, bodyAttr.Value.Invoke(each).HtmlContentToString());
                            }

                            var tr = new TagBuilder("tr");

                            if (this.RowAttributes != null)
                                tr.MergeAttributes(this.RowAttributes);

                            if (this.RowTemplateClass != null)
                                tr.AddCssClass(this.RowTemplateClass.Invoke(each).HtmlContentToString());

                            foreach (var column in this._сolumnList)
                            {
                                var tdWidth = String.IsNullOrWhiteSpace(column.ColumnWidthPct)
                                    ? column.ColumnWidth
                                    : column.ColumnWidthPct;
                                var td = new TagBuilder("td");

                                if (column.Expression != null)
                                {
                                    if (column.IsRaw)
                                        td.InnerHtml.Append("{{{" + column.Expression + "}}}");
                                    else
                                        td.InnerHtml.Append("{{" + column.Expression + "}}");
                                }

                                if (column.Template != null)
                                    td.InnerHtml.AppendHtml(column.Template.Invoke(each));

                                if (column.ColumnAttributes != null)
                                    td.MergeAttributes(column.ColumnAttributes);

                                if (!String.IsNullOrWhiteSpace(tdWidth) &&
                                    (column.ColumnAttributes == null || !column.ColumnAttributes.ContainsKey("style")))
                                    td.MergeAttribute("style",
                                        "width:{0}{1};".F(tdWidth,
                                            String.IsNullOrWhiteSpace(column.ColumnWidthPct) ? "px" : "%"));

                                if (!column.IsVisible)
                                    td.AddCssClass("hide");

                                tr.InnerHtml.AppendHtml(td);
                            }

                            tbody.InnerHtml.AppendHtml(tr);

                            if (this._nextRowList.Count > 0)
                            {
                                foreach (var row in this._nextRowList)
                                {
                                    var trNext = new TagBuilder("tr");

                                    if (row.MetaAttribute != null)
                                    {
                                        trNext.MergeAttributes(row.MetaAttribute(each).AsHtmlAttributes());
                                    }

                                    List<string> classes = row.GetClasses(each);
                                    foreach (var cls in classes)
                                    {
                                        trNext.AddCssClass(cls);
                                    }

                                    if (this.RowAttributes != null)
                                    {
                                        foreach (var att in this.RowAttributes)
                                        {
                                            if (att.Key == "class")
                                                trNext.AddCssClass(att.Value.ToString());
                                        }
                                        trNext.MergeAttributes(this.RowAttributes);
                                    }

                                    if (this.RowTemplateClass != null)
                                        trNext.AddCssClass(this.RowTemplateClass.Invoke(each).HtmlContentToString());

                                    var innerHtml = row.Content.Invoke(each);
                                    
                                    trNext.InnerHtml.AppendHtml(innerHtml);
                                    var trNextString = trNext.HtmlContentToString();
                                    if (row.HtmlAttributes != null)
                                        trNextString = trNextString.Insert(3, " " + row.HtmlAttributes.Invoke(each).HtmlContentToString() + " ");
                                    tbody.InnerHtml.AppendHtml(trNextString);
                                }
                            }
                            tbody.WriteTo(writer, HtmlEncoder.Default);
                        }
                    }
                }
            }

            GridTemplate.Templates.TryAdd(_templateId,
                new IncodingResult.JsonData(true, sb.ToString(), null, HttpStatusCode.OK).ToJsonString());
        }

        private void GeneratePagingTemplate()
        {
            if (_isPageable && string.IsNullOrWhiteSpace(_customPagingTemplateView))
            {
                if (GridTemplate.Templates.ContainsKey(_pagingTemplateId))
                    return;

                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    using (var wrappedHtmlHelper = new HtmlHelperWriterWrapped(_htmlHelper, writer))
                    {
                        using (var template = _htmlHelper.Incoding().Template<PagingModel>())
                        {
                            TagBuilder ul = new TagBuilder("ul");
                            ul.AddCssClass("pagination");

                            //sb.Append("<ul class=\"pagination\">");
                            using (var each = template.ForEach())
                            {
                                using (each.Is(r => r.Active))
                                {
                                    var li = new TagBuilder("li");
                                    var link = new TagBuilder("a");
                                    link.MergeAttribute("href", "#!" + each.For(r => r.Page));
                                    link.InnerHtml.AppendHtml(each.For(r => r.Text));
                                    li.InnerHtml.AppendHtml(link);
                                    li.AddCssClass("active");
                                    ul.InnerHtml.AppendHtml(li);
                                }
                                using (each.Not(r => r.Active))
                                {
                                    var li = new TagBuilder("li");
                                    var link = new TagBuilder("a");
                                    link.MergeAttribute("href", "#!" + each.For(r => r.Page));
                                    link.InnerHtml.AppendHtml(each.For(r => r.Text));
                                    li.InnerHtml.AppendHtml(link);
                                    ul.InnerHtml.AppendHtml(li);
                                }
                            }
                            ul.WriteTo(writer, HtmlEncoder.Default);
                        }
                    }
                }
                GridTemplate.Templates.TryAdd(_pagingTemplateId,
                    new IncodingResult.JsonData(true, sb.ToString(), null, HttpStatusCode.OK).ToJsonString());
            }
        }

        private void AutoBind()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propInfo in properties)
            {
                var attrs = propInfo.FirstOrDefaultAttribute<AutoBindAttribute>();
                AddColumn(new Column<T>(new HtmlString(propInfo.Name))
                {
                    Expression = propInfo.Name,
                    ColumnWidth = attrs != null ?
                                  attrs.Width == 0 ? String.Empty : attrs.Width.ToString() :
                                  String.Empty,

                    ColumnWidthPct = attrs != null ?
                                     attrs.WidthPct == 0 ? String.Empty : attrs.WidthPct.ToString() :
                                     String.Empty,

                    IsRaw = attrs != null && attrs.Raw,
                    IsVisible = attrs == null || !attrs.Hide,
                    Name = new HtmlString(attrs != null ?
                           attrs.Title ?? propInfo.Name :
                           propInfo.Name),

                    SortBy = attrs != null ? attrs.SortBy : null,
                    SortDefault = attrs != null && attrs.SortDefault
                });
            }

        }

        IHtmlContent SortArrow(Action<SortArrowSetting> action)
        {
            var setting = new SortArrowSetting();
            action(setting);

            var link = this._htmlHelper.When(JqueryBind.Click)
                    .PreventDefault().StopPropagation()
                    .OnSuccess(dsl =>
                    {
                        dsl.WithName(this._descSelector).Trigger.Invoke(JqueryBind.Click)
                            .If(() => Selector.Jquery.Name(this._sortBySelector) == setting.By.ToString());
                        if (setting.IsDescDefault)
                            dsl.WithName(this._descSelector).JQuery.Attr.SetProp(HtmlAttribute.Checked)
                                .If(() => Selector.Jquery.Name(this._sortBySelector) != setting.By.ToString());
                        else
                            dsl.WithName(this._descSelector).JQuery.Attr.RemoveProp(HtmlAttribute.Checked)
                                .If(() => Selector.Jquery.Name(this._sortBySelector) != setting.By.ToString());

                        dsl.WithName(this._sortBySelector).JQuery.Attr.Val(setting.By);
                        dsl.WithClass("sort-arrow").Trigger.Incoding();
                        dsl.WithId(setting.TargetId).Trigger.Incoding();
                    })
                    .AsHtmlAttributes()
                    .ToLink(setting.Content);

            TagBuilder tb = new TagBuilder("div");
            tb.InnerHtml.AppendHtml(link);
            tb.InnerHtml.AppendHtml(RenderSortArrow(setting.By, true, setting.SortDefault));
            tb.InnerHtml.AppendHtml(RenderSortArrow(setting.By, false, setting.SortDefault));
            return tb;
        }

        IHtmlContent RenderSortArrow<TEnum>(TEnum sort, bool desc, bool sortDefault)
        {
            string arrowsBootstrap = desc ? "icon-arrow-down" : "icon-arrow-up";

            return this._htmlHelper.When(this._bindEvent)
                    .StopPropagation()
                    .OnSuccess(dsl =>
                               {
                                   dsl.Self().JQuery.Attr.AddClass("hide");
                                   dsl.Self().JQuery.Attr.RemoveClass("hide")
                                           .If(() => Selector.Jquery.Name(this._sortBySelector) == sort.ToString()
                                                     &&
                                                     Selector.Jquery.Name(this._descSelector) == desc);
                               })
                                .AsHtmlAttributes(new { @class = arrowsBootstrap + " sort-arrow hide" })
                                .ToTag(HtmlTag.I);
        }


        #endregion

        public void AddColumn(Column<T> column)
        {
            this._сolumnList.Add(column);
        }

        public void SetItemsCount(bool showItemsCount)
        {
            _showItemsCount = showItemsCount;
        }

        public void SetPageSizes(bool showPageSizes)
        {
            _showPageSizes = showPageSizes;
        }

        public void SetPageSizesArray(params int[] pageSizesArray)
        {
            _pageSizesArray = pageSizesArray;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Render().WriteTo(writer, encoder);
        }
    }

}