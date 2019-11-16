using System;
using Generic.Repository.Enums;
using Generic.Repository.Validations.ThrowError;

namespace Generic.Repository.Models.PageAggregation.PageConfig
{
    public class PageConfig : IPageConfig, IEquatable<PageConfig>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public PageSort Sort { get; set; }
        public string Order { get; set; }

        public PageConfig()
        {
            Page = 0;
            Size = 10;
            Sort = PageSort.DESC;
            Order = "Id";
        }

        public PageConfig(string order)
        {
            Page = 0;
            Size = 10;
            Sort = PageSort.DESC;
            Order = order;
        }

        public bool Equals(PageConfig pageConfig)
        {
            ThrowErrorIf.
               IsNullValue(pageConfig, nameof(pageConfig), nameof(Equals));

            return pageConfig.Equals(this);
        }
    }
}
