using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Repository.Models.Page
{
    public interface IPage<TReturn> : IEquatable<TReturn>
     where TReturn : class
    {
        Task<IReadOnlyList<TReturn>> Content { get; }

        int TotalElements { get; }

        string Sort { get; }

        string Order { get; }

        int Size { get; }

        int NumberPage { get; }

        int TotalPage { get; }
    }
}
