using System;
using System.Collections.Generic;

namespace Generic.Repository.Models.Page
{
    public interface IPage<TReturn> : IEquatable<TReturn>
     where TReturn : class
    {
        IReadOnlyList<TReturn> Content { get; }

        int TotalElements { get; }

        string Sort { get; }

        string Order { get; }

        int Size { get; }

        int NumberPage { get; }

        int TotalPage { get; }
    }
}
