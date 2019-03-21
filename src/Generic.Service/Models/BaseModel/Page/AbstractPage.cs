using System;
using System.Collections.Generic;
using System.Linq;
using Generic.Service.Extensions.Commom;
using Generic.Service.Models.BaseModel.Page.PageConfiguration;

namespace Generic.Service.Models.BaseModel.Page
{
public abstract class AbstractPage<TValue, TResult> : IPage<TResult>
        where TValue : class
    where TResult : class {
        #region Default Parameters
        protected readonly Func<IEnumerable<TValue>, IEnumerable<TResult>> _mapperTo;
        protected readonly bool _pageStatsInOne;
        protected readonly string _defaultSort;
        protected readonly string _defaultOrder;
        protected readonly int _defaultSize;
        #endregion
        #region Parameters Ctor
        protected readonly IPageConfiguration _config;
        protected readonly IQueryable<TValue> _listEntities;
        protected readonly int _count;
        #endregion

        #region Ctor
        public AbstractPage (IQueryable<TValue> listEntities, Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo, IPageConfiguration config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize) {
            if (mapperTo == null)
                throw new ArgumentNullException ($"ERROR> NameClass: {nameof(AbstractPage<TValue, TResult>)} Message: The delegate {nameof(mapperTo)} is not can be null.");
            _mapperTo = mapperTo;
            _count = listEntities.Count ();
            ValidateCtor (_count, listEntities, config);
            _config = config;
            _listEntities = listEntities;
            _pageStatsInOne = pageStartInOne;
            _defaultOrder = defaultOrder;
            _defaultSize = defaultSize;
            _defaultSort = defaultSort;
        }
        #endregion

        protected virtual void ValidateCtor (int count, IQueryable<TValue> listEntities, IPageConfiguration config) {
            if (count < 1 || config == null)
                throw new ArgumentNullException ($"ERROR> NameClass: {nameof(ValidateCtor)}. {Environment.NewLine}Message: The {(config != null ? nameof(listEntities) : nameof(config))} is empty!");
        }

        public virtual IEnumerable<TResult> Content {
            get {
                IQueryable<TValue> queryableE = _listEntities.Skip (NumberPage * TotalElements).Take (Size);

                queryableE = Sort == "ASC" ? queryableE.OrderBy (x => Commom.CacheGet[typeof (TValue).Name][Order] (x)) :
                    queryableE.OrderByDescending (x => Commom.CacheGet[typeof (TValue).Name][Order] (x));

                return _mapperTo (queryableE);
            }
        }

        public virtual int TotalElements {
            get => _count;
        }

        public virtual string Sort {
            get => _config.sort ?? _defaultSort;
        }

        public virtual string Order {
            get => _config.order ?? _defaultOrder;
        }

        public virtual int Size {
            get => _config.size == 0 ? _defaultSize : _config.size;
        }

        public virtual int NumberPage {
            get => _pageStatsInOne ? _config.page - 1 : _config.page;
        }
    }

    public abstract class AbstractPage<TValue> : AbstractPage<TValue, TValue>
        where TValue : class {
            #region Ctor
            public AbstractPage (IQueryable<TValue> listEntities, IPageConfiguration config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize) : base (listEntities, null, config, pageStartInOne, defaultSort, defaultOrder, defaultSize) { }
            #endregion

            public override IEnumerable<TValue> Content {
                get {
                    IQueryable<TValue> queryable = _listEntities.Skip (NumberPage * TotalElements).Take (Size);

                    queryable = Sort == "ASC" ? queryable.OrderBy (x => Commom.CacheGet[typeof (TValue).Name][Order] (x)) :
                        queryable.OrderByDescending (x => Commom.CacheGet[typeof (TValue).Name][Order] (x));

                    return queryable;
                }
            }

        }
}