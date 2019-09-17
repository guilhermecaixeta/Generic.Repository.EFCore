using Generic.Repository.Cache;
using Generic.Repository.Models.Page.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Models.Page
{
    /// <summary>
    /// Page Class
    /// Default values:
    /// sort = ASC
    /// order = Id
    /// </summary>
    /// <typeparam name="TValue">Type of page return</typeparam>
    public class Page<TValue> : AbstractPage<TValue>
    where TValue : class
    {
        public Page(
        ICacheRepository cacheRepository,
        IQueryable<TValue> listEntities,
        IPageConfig config,
        bool pageStartInOne,
        string defaultSort,
        string defaultOrder,
        int defaultSize
        ) :
        base(
        cacheRepository,
        listEntities,
        config,
        pageStartInOne,
        defaultSort,
        defaultOrder,
        defaultSize)
        { }
    }

    public class Page<TValue, TResult> : AbstractPage<TValue, TResult>
    where TValue : class
    where TResult : class
    {
        public Page(
                ICacheRepository cacheRepository,
                IQueryable<TValue> listEntities,
                Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo,
                IPageConfig config,
                bool pageStartInOne,
                string defaultSort,
                string defaultOrder,
                int defaultSize
            ) :
            base(
                cacheRepository,
                listEntities,
                mapperTo,
                config,
                pageStartInOne,
                defaultSort,
                defaultOrder,
                defaultSize
            )
        { }
    }
}
