using Generic.Repository.Enums;

namespace Generic.Repository.Models.PageAggregation.PageConfig
{
    public interface IPageConfig
    {
        int Page { get; set; }
        int Size { get; set; }
        PageSort Sort { get; set; }
        string Order { get; set; }
    }
}
