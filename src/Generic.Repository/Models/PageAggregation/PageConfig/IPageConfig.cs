using Generic.Repository.Enums;

namespace Generic.Repository.Models.PageAggregation.PageConfig
{
    public interface IPageConfig
    {
        string Order { get; set; }
        int Page { get; set; }
        int Size { get; set; }
        PageSort Sort { get; set; }
    }
}