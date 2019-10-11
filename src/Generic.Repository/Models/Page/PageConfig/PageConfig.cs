using System;
using Generic.Repository.ThrowError;

namespace Generic.Repository.Models.Page.PageConfig
{
    public class PageConfig : IPageConfig, IEquatable<PageConfig>
    {
        public int page { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public string order { get; set; }

        public bool Equals(PageConfig pageConfig)
        {
             ThrowErrorIf.
                IsNullValue(pageConfig, nameof(pageConfig), nameof(Equals));

            return pageConfig.Equals(this);
        }
    }
}
