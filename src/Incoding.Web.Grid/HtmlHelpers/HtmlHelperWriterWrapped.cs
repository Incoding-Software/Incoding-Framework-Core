using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.Grid.HtmlHelpers
{
    public class HtmlHelperWriterWrapped : IDisposable
    {
        private TextWriter _originalWriter;
        public IHtmlHelper HtmlHelper { get; }
        public TextWriter Writer { get; }

        public HtmlHelperWriterWrapped(IHtmlHelper htmlHelper, TextWriter writer)
        {
            HtmlHelper = htmlHelper;
            Writer = writer;
            _originalWriter = htmlHelper.ViewContext.Writer;
            HtmlHelper.ViewContext.Writer = writer;
        }

        public void Dispose()
        {
            HtmlHelper.ViewContext.Writer = _originalWriter;
        }
    }
    
}