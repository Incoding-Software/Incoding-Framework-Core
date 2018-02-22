using System;
using System.ComponentModel;

namespace Incoding.Web.Grid.Components
{
    [Flags]
    public enum BootstrapTable
    {
        [Description("table-striped")]
        Striped = 1,
        [Description("table-bordered")]
        Bordered = 2,
        [Description("table-hover")]
        Hover = 4,
        [Description("table-condensed")]
        Condensed = 8
    }
}
