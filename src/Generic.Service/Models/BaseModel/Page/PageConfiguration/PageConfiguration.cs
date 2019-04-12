using System;
using Generic.Service.Extensions.Validation;

namespace Generic.Service.Models.BaseModel.Page.PageConfiguration
{
    public struct PageConfiguration : IPageConfiguration, IEquatable<PageConfiguration>
    {
        public int page { get ; set ; }
        public int size { get ; set ; }
        public string sort { get ; set ; }
        public string order { get ; set ; }

        public bool Equals(PageConfiguration other)
        {
            other.IsNull(nameof(Equals),nameof(other));
            return other.Equals(this);
        }
    }
}