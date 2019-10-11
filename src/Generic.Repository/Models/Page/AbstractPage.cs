using Generic.Repository.Cache;
using Generic.Repository.Models.Page.PageConfig;
using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Models.Page
{
    public abstract class AbstractPage<TValue, TResult> : IPage<TResult>
    where TValue : class
    where TResult : class
    {
        #region Const and Readonly

        /// <summary>The cache repository.</summary>
        private readonly ICacheRepository _cacheRepository;

        /// <summary>The sort asc</summary>
        private const string ASC = nameof(ASC);
        #endregion

        #region Default Parameters
        protected readonly Func<IEnumerable<TValue>, IEnumerable<TResult>> MapperTo;
        protected readonly bool PageStatsInOne;
        protected readonly string DefaultSort;
        protected readonly string DefaultOrder;
        protected readonly int DefaultSize;
        #endregion

        #region Parameters Ctor
        protected readonly IPageConfig Config;
        protected readonly IQueryable<TValue> ListEntities;
        protected readonly int Count;
        #endregion

        #region Ctor
        protected AbstractPage(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo,
            IPageConfig config,
            bool pageStartInOne,
            string defaultSort,
            string defaultOrder,
            int defaultSize
        )
        {
            _cacheRepository = cacheRepository;
            MapperTo = mapperTo;
            Count = listEntities.Count();
            IsPageConfigValid(config);
            Config = config;
            ListEntities = listEntities;
            PageStatsInOne = pageStartInOne;
            DefaultOrder = defaultOrder;
            DefaultSize = defaultSize;
            DefaultSort = defaultSort;
        }
        #endregion

        /// <summary>Determines whether [is page configuration valid] [the specified configuration].</summary>
        /// <param name="config">The configuration.</param>
        private static void IsPageConfigValid(IPageConfig config)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(IsPageConfigValid));
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="result">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="result" /> parameter; otherwise, false.</returns>
        public bool Equals(TResult result)
        {
            ThrowErrorIf.IsNullValue(result, nameof(result), nameof(Equals));
            return result == this;
        }

        /// <summary>Gets the content.</summary>
        /// <value>The content of page.</value>
        public virtual IReadOnlyList<TResult> Content => MapperTo(GetItems()).ToList();

        /// <summary>Gets the total elements.</summary>
        /// <value>The total elements in a page.</value>
        public virtual int TotalElements => Count;

        /// <summary>Gets the sort.</summary>
        /// <value>The pagination sort.</value>
        public virtual string Sort =>
            Config.sort ?? DefaultSort;

        /// <summary>Gets the order.</summary>
        /// <value>The order of page.</value>
        public virtual string Order =>
            Config.order ?? DefaultOrder;

        /// <summary>Gets the size.</summary>
        /// <value>The size of page.</value>
        public virtual int Size =>
            Config.size == 0 ? DefaultSize : Config.size;

        /// <summary>Gets the number page.</summary>
        /// <value>The number page.</value>
        public virtual int NumberPage =>
            PageStatsInOne ? Config.page - 1 : Config.page;

        /// <summary>Gets the total page.</summary>
        /// <value>The total page.</value>
        public virtual int TotalPage =>
            TotalElements / Size;

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected IReadOnlyList<TValue> GetItems()
        {
            var methodGet = _cacheRepository.
                GetMethodGet(typeof(TValue).Name, Order);

            var list = !Sort.ToUpper().Equals(ASC)
                ? ListEntities.OrderByDescending(x => methodGet(x))
                : ListEntities.OrderBy(x => methodGet(x));

            var skipNumber = NumberPage * Size;

            var result = list.Skip(skipNumber).Take(Size);

            return result.ToList();
        }
    }

    public abstract class AbstractPage<TValue> : AbstractPage<TValue, TValue>
        where TValue : class
    {
        #region Ctor

        protected AbstractPage(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config, bool pageStartInOne,
            string defaultSort, string defaultOrder,
            int defaultSize
        ) :
            base(
                cacheRepository,
                listEntities,
                null,
                config,
                pageStartInOne,
                defaultSort,
                defaultOrder,
                defaultSize
            )
        {
        }

        #endregion

        public override IReadOnlyList<TValue> Content => GetItems();

    }
}
