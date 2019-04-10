using System;

namespace Generic.Service.Models.BaseModel.Page
{
    public interface IPageConfiguration
    {
        int page { get; set; }
        int size { get; set; }
        string sort { get; set; }
        string order { get; set; }
    }
}