using System;
using Generic.Repository.Extension.Validation;

namespace Generic.Repository.Models.Page.PageConfig
{
    public class PageConfig : IPageConfig, IEquatable<PageConfig>
    {
        public int page { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public string order { get; set; }

        public bool Equals(PageConfig other)
        {
            other.IsNull(nameof(Equals), nameof(other));
            return other.Equals(this);
        }
    }
}
