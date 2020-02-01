using Generic.Repository.Cache;
using Generic.Repository.Enums;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Models.PageAggregation
{
    public abstract class PageAttrAbstract<TIn, TOut> : IPage<TOut>
        where TIn : class
        where TOut : class
    {
        public PageAttrAbstract(IQueryable<TIn> listEntities, IPageConfig config, ICacheRepository cache)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), string.Empty);

            ThrowErrorIf.IsNullValue(listEntities, nameof(listEntities), string.Empty);

            ThrowErrorIf.IsNullValue(cache, nameof(cache), string.Empty);

            Cache = cache;
            PageConfig = config;
            ListEntities = listEntities;
            Count = listEntities.Count();

            Sort = PageConfig.Sort.ToString();
            NumberPage = PageConfig.Page;
            Order = PageConfig.Order;
            Size = PageConfig.Size;
            TotalElements = Count;

            TotalPage = TotalElements / Size;
        }

        #region PARAMETERS CTOR

        /// <summary>The cache repository.</summary>
        protected ICacheRepository Cache;

        /// <summary>The count</summary>
        protected int Count;

        /// <summary>The list entities</summary>
        protected IQueryable<TIn> ListEntities;

        /// <summary>The page configuration</summary>
        protected IPageConfig PageConfig;

        #endregion PARAMETERS CTOR

        /// <summary>Gets the content.</summary>
        /// <value>The content of page.</value>
        public virtual IReadOnlyList<TOut> Content { get; set; }

        /// <summary>Gets the number page.</summary>
        /// <value>The number page.</value>
        public virtual int NumberPage { get; set; }

        /// <summary>Gets the order.</summary>
        /// <value>The order of page.</value>
        public virtual string Order { get; set; }

        /// <summary>Gets the size.</summary>
        /// <value>The size of page.</value>
        public virtual int Size { get; set; }

        /// <summary>Gets the sort.</summary>
        /// <value>The pagination sort.</value>
        public virtual string Sort { get; set; }

        /// <summary>Gets the total elements.</summary>
        /// <value>The total elements in a page.</value>
        public virtual int TotalElements { get; set; }

        /// <summary>Gets the total page.</summary>
        /// <value>The total page.</value>
        public virtual int TotalPage { get; set; }

        public abstract Task<IPage<TOut>> Init(CancellationToken token);

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected virtual async Task<IReadOnlyList<TIn>> GetItems(CancellationToken token)
        {
            var result = await GetQueryable(token).
                ConfigureAwait(false);

            return await result.ToListAsync(token).
                ConfigureAwait(false);
        }

        protected async Task<IQueryable<TIn>> GetQueryable(CancellationToken token)
        {
            var orderBy = await PageConfig.
                        CreateGenericOrderBy<TIn>(Cache, token).
                        ConfigureAwait(false);

            var list = !Sort.Equals(PageSort.ASC.ToString())
            ? ListEntities.OrderByDescending(orderBy)
            : ListEntities.OrderBy(orderBy);

            var skipNumber = NumberPage * Size;

            var result = list.Skip(skipNumber).Take(Size);
            return result;
        }
    }
}