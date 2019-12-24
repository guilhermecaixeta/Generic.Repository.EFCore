using Generic.Repository.Enums;
using Generic.Repository.Validations.ThrowError;
using System;

namespace Generic.Repository.Models.PageAggregation.PageConfig
{
    public class PageConfig : IPageConfig, IEquatable<PageConfig>
    {
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

        public string Order { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public PageSort Sort { get; set; }

        public bool Equals(PageConfig pageConfig)
        {
            ThrowErrorIf.
               IsNullValue(pageConfig, nameof(pageConfig), nameof(Equals));

            return pageConfig.Equals(this);
        }
    }
}