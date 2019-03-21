using System;
using System.Collections.Generic;
using System.Linq;
using Generic.Service.Models.BaseModel.Page.PageConfiguration;

namespace Generic.Service.Models.BaseModel.Page
{
    /// <summary>
    /// Page Class
    /// Default values:
    /// sort = ASC
    /// order = Id
    /// </summary>
    /// <typeparam name="TValue">Type of page return</typeparam>
    public class Page<TValue> : AbstractPage<TValue>
        where TValue : class {
            public Page (IQueryable<TValue> listEntities, IPageConfiguration config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize) 
            : base (listEntities, config, pageStartInOne, defaultSort, defaultOrder, defaultSize) { }
        }

    public class Page<TValue, TResult> : AbstractPage<TValue, TResult>
        where TValue : class
    where TResult : class {
        public Page (IQueryable<TValue> listEntities, Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo, IPageConfiguration config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize) : base (listEntities, mapperTo, config, pageStartInOne, defaultSort, defaultOrder, defaultSize) { }
    }
}