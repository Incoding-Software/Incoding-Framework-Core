using System.Collections.Generic;
using System.Linq;

namespace Incoding.Core.ViewModel
{
    #region << Using >>

    #endregion

    public class OptGroupVm
    {
        #region Constructors

        public OptGroupVm(IEnumerable<KeyValueVm> items)
                : this(string.Empty, items) { }

        public OptGroupVm(string title, IEnumerable<KeyValueVm> items)
        {
            Items = items.ToList();
            Title = string.IsNullOrWhiteSpace(title) ? null : title;
        }

        #endregion

        #region Properties

        public IList<KeyValueVm> Items { get; set; }

        public string Title { get; set; }

        #endregion
    }
}