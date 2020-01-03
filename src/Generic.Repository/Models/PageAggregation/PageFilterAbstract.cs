using Generic.Repository.Cache;
using Generic.Repository.Enums;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Repository.Models.PageAggregation
{
    public abstract class PageAbstract<TValue> : PageAbstract<TValue, TValue>
        where TValue : class
    {
        protected PageAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config
        ) : base(cacheRepository, listEntities, config, null) { }

        protected override bool NeedMap { get; } = false;

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected override async Task<IReadOnlyList<TValue>> GetItems()
        {
            var result = await GetQueryable().ConfigureAwait(false);
            return await result.ToListAsync().ConfigureAwait(false);
        }
    }

    public abstract class PageAbstract<TValue, TResult> : IPage<TResult>
        where TValue : class
        where TResult : class
    {
        #region Const and Readonly

        /// <summary>The sort asc</summary>
        protected const string ASC = nameof(ASC);

        /// <summary>The cache repository.</summary>
        protected readonly ICacheRepository CacheRepository;

        /// <summary>The mapping</summary>
        protected readonly Func<IEnumerable<object>, IEnumerable<TResult>> Mapping;

        protected virtual bool NeedMap { get; } = true;

        #endregion Const and Readonly

        #region Parameters Ctor

        protected readonly int Count;
        protected readonly IQueryable<TValue> ListEntities;
        protected readonly IPageConfig PageConfig;

        #endregion Parameters Ctor

        #region Ctor

        public PageAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping
        )
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), typeof(PageAbstract<,>).Name);
            ThrowErrorIf.IsNullValue(listEntities, nameof(listEntities), typeof(PageAbstract<,>).Name);
            ThrowErrorIf.IsNullValue(cacheRepository, nameof(cacheRepository), typeof(PageAbstract<,>).Name);
            if (NeedMap)
            {
                ThrowErrorIf.IsNullValue(mapping, nameof(mapping), typeof(PageAbstract<,>).Name);
            }

            Mapping = mapping;
            PageConfig = config;
            ListEntities = listEntities;
            Count = listEntities.Count();
            CacheRepository = cacheRepository;

            Sort = PageConfig.Sort.ToString();
            NumberPage = PageConfig.Page;
            Order = PageConfig.Order;
            Size = PageConfig.Size;
            TotalElements = Count;

            TotalPage = TotalElements / Size;
            Content = GetItems().
                        ConfigureAwait(false).
                        GetAwaiter().
                        GetResult();
        }

        #endregion Ctor

        /// <summary>Gets the content.</summary>
        /// <value>The content of page.</value>
        public virtual IReadOnlyList<TResult> Content { get; set; }

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

        internal async Task<IQueryable<TValue>> GetQueryable()
        {
            var orderBy = await PageConfig.CreateGenericOrderBy<TValue>(CacheRepository, default);

            var list = !Sort.Equals(PageSort.ASC.ToString())
            ? ListEntities.OrderByDescending(orderBy)
            : ListEntities.OrderBy(orderBy);

            var skipNumber = NumberPage * Size;

            var result = list.Skip(skipNumber).Take(Size);
            return result;
        }

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected virtual async Task<IReadOnlyList<TResult>> GetItems()
        {
            var result = await GetQueryable();

            return Mapping(result).ToList();
        }
    }

    public abstract class PageFilterAbstract<TValue, TFilter, TResult> : IPage<TResult>
            where TValue : class
    where TFilter : class, IFilter
    where TResult : class
    {
        #region Const and Readonly

        /// <summary>The sort asc</summary>
        private const string ASC = nameof(ASC);

        /// <summary>The cache repository.</summary>
        private readonly ICacheRepository _cacheRepository;

        #endregion Const and Readonly

        #region Parameters Ctor

        protected readonly int Count;
        protected readonly IQueryable<TValue> ListEntities;
        protected readonly IPageConfig PageConfig;

        #endregion Parameters Ctor

        #region Ctor

        protected PageFilterAbstract(
            IPageConfig config,
            IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapperTo,
            bool needMap = true
        )
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), string.Empty);
            ThrowErrorIf.IsNullValue(listEntities, nameof(listEntities), string.Empty);
            ThrowErrorIf.IsNullValue(cacheRepository, nameof(cacheRepository), string.Empty);

            PageConfig = config;
            ListEntities = listEntities;
            Count = listEntities.Count();
            _cacheRepository = cacheRepository;

            Sort = PageConfig.Sort.ToString();
            NumberPage = PageConfig.Page;
            Order = PageConfig.Order;
            Size = PageConfig.Size;
            TotalElements = Count;

            TotalPage = TotalElements / Size;

            if (needMap)
            {
                ThrowErrorIf.IsNullValue(mapperTo, nameof(mapperTo), string.Empty);
                Content = mapperTo(GetItems()).ToList();
            }
        }

        #endregion Ctor

        /// <summary>Gets the content.</summary>
        /// <value>The content of page.</value>
        public virtual IReadOnlyList<TResult> Content { get; set; }

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

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected IReadOnlyList<TValue> GetItems()
        {
            var orderBy = PageConfig.CreateGenericOrderBy<TValue, TFilter>(_cacheRepository, default).GetAwaiter().GetResult();

            var list = !Sort.Equals(PageSort.ASC.ToString())
            ? ListEntities.OrderByDescending(orderBy)
            : ListEntities.OrderBy(orderBy);

            var skipNumber = NumberPage * Size;

            var result = list.Skip(skipNumber).Take(Size);

            return result.ToList();
        }
    }

    public abstract class PageFilterAbstract<TValue, TFilter> : PageFilterAbstract<TValue, TFilter, TValue>
        where TValue : class
        where TFilter : class, IFilter
    {
        #region Ctor

        protected PageFilterAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config
        ) :
            base(
                config,
                listEntities,
                cacheRepository,
                null,
                false)
        {
        }

        #endregion Ctor

        public override IReadOnlyList<TValue> Content => GetItems();
    }
}