using System;
using System.Collections.Generic;

namespace Generic.Repository.Models.Page
{
    public interface IPage<TReturn>
     where TReturn : class
    {
        IReadOnlyList<TReturn> Content { get; set; }

        int TotalElements { get; set; }

        string Sort { get; set; }

        string Order { get; set; }

        int Size { get; set; }

        int NumberPage { get; set; }

        int TotalPage { get; set; }
    }
}
