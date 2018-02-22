using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Extensions;
using Incoding.Web.Extensions;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Incoding.Web.Grid.GridParts
{
    public class Row<T> where T : class
    {
        #region Constructors

        public Row(Func<ITemplateSyntax<T>, HelperResult> content)
        {
            this.Content = content;
        }

        #endregion

        #region Properties

        public Row() { }

        public Func<ITemplateSyntax<T>, HelperResult> Content { get; set; }

        public Func<ITemplateSyntax<T>, HelperResult> HtmlAttributes { get; set; }

        public Func<ITemplateSyntax<T>, IIncodingMetaLanguageEventBuilderDsl> MetaAttribute { get; set; }

        List<Func<ITemplateSyntax<T>, HelperResult>> funcClasses = new List<Func<ITemplateSyntax<T>, HelperResult>>();
        List<string> classes = new List<string>();

        internal List<string> GetClasses(ITemplateSyntax<T> each)
        {
            List<string> clss = new List<string>();
            clss.AddRange(classes);
            clss.AddRange(funcClasses.Select(r => r.Invoke(each).HtmlContentToString()));
            return clss;
        }

        public void AddClass(Func<ITemplateSyntax<T>, HelperResult> cls)
        {
            funcClasses.Add(cls);
        }
        public void AddClass(string cls)
        {
            classes.Add(cls);
        }
        public void AddClass(B cls)
        {
            classes.Add(cls.ToLocalization());
        }

        #endregion
    }
}