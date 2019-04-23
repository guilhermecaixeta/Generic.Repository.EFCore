using System;

namespace Generic.Repository.Models.Page.PageConfig
{
    public interface IPageConfig
    {
        int page { get; set; }
        int size { get; set; }
        string sort { get; set; }
        string order { get; set; }
    }
}
