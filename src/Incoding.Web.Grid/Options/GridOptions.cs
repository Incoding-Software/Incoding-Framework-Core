using System;
using System.Linq;
using Incoding.Core.Extensions;
using Incoding.Web.Grid.Components;
using Incoding.Web.MvcContrib;

namespace Incoding.Web.Grid.Options
{
    public class GridOptions
    {
        public static readonly GridOptions Default = new GridOptions();

        private string _styling = null;

        public Selector NoRecordsSelector { get; set; }

        public virtual void AddStyling(string @class)
        {
            _styling = @class;
        }

        public virtual void AddStyling(BootstrapTable @class)
        {
            var classes = Enum.GetValues(typeof(BootstrapTable))
                .Cast<BootstrapTable>()
                .Where(r => @class.HasFlag(r))
                .Select(r => r.ToLocalization())
                .AsString(" ");

            AddStyling(classes);
        }

        public virtual string GetStyling()
        {
            return _styling;
        }
    }
}