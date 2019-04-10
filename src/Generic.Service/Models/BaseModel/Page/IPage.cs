using System;
using System.Collections.Generic;

namespace Generic.Service.Models.BaseModel.Page
{
    public interface IPage<TReturn> : IEquatable<TReturn>
     where TReturn : class
    {
        List<TReturn> Content { get; }

        int TotalElements { get; }

        string Sort { get; }

        string Order { get; }

        int Size { get; }

        int NumberPage { get; }

        int TotalPage { get; }
    }
}