using Generic.Repository.Cache;
using Generic.Repository.Extension.Validation;
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
        private readonly ICacheRepository _cacheRepository;

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
            ValidateCtor(_count, config);
            _config = config;
            _listEntities = listEntities;
            _pageStatsInOne = pageStartInOne;
            _defaultOrder = defaultOrder;
            _defaultSize = defaultSize;
            _defaultSort = defaultSort;
        }
        #endregion

        private static void ValidateCtor(int count, IPageConfig config)
        {
            config.IsNull(nameof(ValidateCtor), nameof(config));
            if (count < 1)
            {
                Validation.HandleNullError($"ClassName: {nameof(ValidateCtor)} {Environment.NewLine}Message: The listEntities is empty!");
            }
        }

        public bool Equals(TResult other)
        {
            other.IsNull(nameof(Equals), nameof(other));
            return other == this;
        }

        public virtual IReadOnlyList<TResult> Content
        {
            get
            {
                _mapperTo.IsNull(nameof(AbstractPage<TValue, TResult>), nameof(_mapperTo));
                return _mapperTo(GetItems()).ToList();
            }
        }

        public virtual int TotalElements
        {
            get => _count;
        }

        public virtual string Sort
        {
            get => _config.sort ?? _defaultSort;
        }

        public virtual string Order
        {
            get => _config.order ?? _defaultOrder;
        }

        public virtual int Size
        {
            get => _config.size == 0 ? _defaultSize : _config.size;
        }

        public virtual int NumberPage
        {
            get => _pageStatsInOne ? _config.page - 1 : _config.page;
        }

        public virtual int TotalPage
        {
            get => TotalElements / Size;
        }

        protected IQueryable<TValue> GetItems()
        {
            IQueryable<TValue> dataList = !Sort.ToLower().Equals("asc") ?
            _listEntities.
            OrderByDescending(x =>
                _cacheRepository.
                GetMethodGet(typeof(TValue).Name, Order)(x))
            :
            _listEntities.
            OrderBy(x =>
                _cacheRepository.
                GetMethodGet(typeof(TValue).Name, Order)(x));
            return dataList.
            Skip(NumberPage * Size).
            Take(Size);
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
