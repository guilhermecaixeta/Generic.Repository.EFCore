using Generic.Repository.Extension.Error;
using System;

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
            other.ThrowErrorNullValue(nameof(Equals), nameof(other));
            return other.Equals(this);
        }
    }
}
