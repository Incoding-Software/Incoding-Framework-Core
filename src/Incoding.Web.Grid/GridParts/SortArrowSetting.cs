using Microsoft.AspNetCore.Html;

namespace Incoding.Web.Grid.GridParts
{
    public class SortArrowSetting
    {
        #region Properties

        public object By { get; set; }
        public string TargetId { get; set; }
        public IHtmlContent Content { get; set; }
        public bool SortDefault { get; set; }
        public bool IsDescDefault { get; set; }

        #endregion
    }
}