using System.Collections.Generic;

namespace Generic.Repository.Models.PageAggregation
{
    public interface IPage<TReturn>
     where TReturn : class
    {
        IReadOnlyList<TReturn> Content { get; set; }

        int NumberPage { get; set; }
        string Order { get; set; }
        int Size { get; set; }
        string Sort { get; set; }
        int TotalElements { get; set; }
        int TotalPage { get; set; }
    }
}