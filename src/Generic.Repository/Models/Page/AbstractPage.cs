using Generic.Repository.Cache;
using Generic.Repository.Extension.Error;
using Generic.Repository.Models.Page.PageConfig;
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

        /// <summary>The ordenation asc</summary>
        private const string ASC = nameof(ASC);
        #endregion

        #region Default Parameters
        protected readonly Func<IEnumerable<TValue>, IEnumerable<TResult>> _mapperTo;
        protected readonly bool _pageStatsInOne;
        protected readonly string _defaultSort;
        protected readonly string _defaultOrder;
        protected readonly int _defaultSize;
        #endregion

        #region Parameters Ctor
        protected readonly IPageConfig _config;
        protected readonly IQueryable<TValue> _listEntities;
        protected readonly int _count;
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
            _mapperTo = mapperTo;
            _count = listEntities.Count();
            IsPageConfigValid(config);
            _config = config;
            _listEntities = listEntities;
            _pageStatsInOne = pageStartInOne;
            _defaultOrder = defaultOrder;
            _defaultSize = defaultSize;
            _defaultSort = defaultSort;
        }
        #endregion

        /// <summary>Determines whether [is page configuration valid] [the specified configuration].</summary>
        /// <param name="config">The configuration.</param>
        private static void IsPageConfigValid(IPageConfig config)
        {
            config.ThrowErrorNullValue(nameof(config), nameof(IsPageConfigValid));
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(TResult other)
        {
            other.ThrowErrorNullValue(nameof(other), nameof(Equals));
            return other == this;
        }

        /// <summary>Gets the content.</summary>
        /// <value>The content of page.</value>
        public virtual IReadOnlyList<TResult> Content
        {
            get
            {
                _mapperTo.ThrowErrorNullValue(nameof(_mapperTo), nameof(AbstractPage<TValue, TResult>));
                return _mapperTo(GetItems()).ToList();
            }
        }

        /// <summary>Gets the total elements.</summary>
        /// <value>The total elements in a page.</value>
        public virtual int TotalElements
        {
            get => _count;
        }

        /// <summary>Gets the sort.</summary>
        /// <value>The pagination sort.</value>
        public virtual string Sort
        {
            get => _config.sort ?? _defaultSort;
        }

        /// <summary>Gets the order.</summary>
        /// <value>The order of page.</value>
        public virtual string Order
        {
            get => _config.order ?? _defaultOrder;
        }

        /// <summary>Gets the size.</summary>
        /// <value>The size of page.</value>
        public virtual int Size
        {
            get => _config.size == 0 ? _defaultSize : _config.size;
        }

        /// <summary>Gets the number page.</summary>
        /// <value>The number page.</value>
        public virtual int NumberPage
        {
            get => _pageStatsInOne ? _config.page - 1 : _config.page;
        }

        /// <summary>Gets the total page.</summary>
        /// <value>The total page.</value>
        public virtual int TotalPage
        {
            get => TotalElements / Size;
        }

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected IQueryable<TValue> GetItems()
        {
            var get = _cacheRepository.
                GetMethodGet(typeof(TValue).Name, Order);

            var list = !Sort.ToUpper().Equals(ASC) ?
            _listEntities.OrderByDescending(x => get(x))
            : _listEntities.OrderBy(x => get(x));

            var skipNumber = NumberPage * Size;

            var result = list.Skip(skipNumber).Take(Size);

            return result;
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
        { }

        #endregion

        public override IReadOnlyList<TValue> Content
        {
            get
            {
                return GetItems().ToList();
            }
        }
    }
}
