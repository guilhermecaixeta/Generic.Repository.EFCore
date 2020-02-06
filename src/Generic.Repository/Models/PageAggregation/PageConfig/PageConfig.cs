using Generic.Repository.Enums;
using Generic.Repository.ThrowError;
using System;

namespace Generic.Repository.Models.PageAggregation.PageConfig
{
    /// <summary>
    /// Page configuration
    /// </summary>
    /// <seealso cref="Generic.Repository.Models.PageAggregation.PageConfig.IPageConfig" />
    /// <seealso cref="System.IEquatable{Generic.Repository.Models.PageAggregation.PageConfig.PageConfig}" />
    public class PageConfig : IPageConfig, IEquatable<PageConfig>
    {
        /// <summary>Initializes a new instance of the <see cref="PageConfig"/> class.</summary>
        public PageConfig()
        {
            Page = 0;
            Size = 10;
            Sort = PageSort.DESC;
            Order = "Id";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageConfig"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
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

        public bool Equals(PageConfig other)
        {
            ThrowErrorIf.
                IsNullValue(other, nameof(other), nameof(Equals));

            return other == this;
        }
    }
}