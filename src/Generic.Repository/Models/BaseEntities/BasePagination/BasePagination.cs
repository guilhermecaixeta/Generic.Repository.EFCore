using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Base.Pagination
{
    /// <summary>
    /// Pagination Class
    /// Default values:
    /// sort = ASC
    /// order = Id
    /// </summary>
    /// <typeparam name="E">Type of pagination</typeparam>
    public class Pagination<E>
        where E : class
    {
        #region Default Parameters
        private readonly bool _pageStatsInOne;
        private readonly string _defaultSort;
        private readonly string _defaultOrder;
        private readonly int _defaultSize;
        #endregion
        #region Parameters Ctor
        private readonly BaseConfigurePagination _config;
        private readonly IQueryable<E> _listEntities;
        private readonly int _count;
        #endregion

        #region Ctor
        public Pagination(IQueryable<E> listEntities, BaseConfigurePagination config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize)
        {
            _count = listEntities.Count();
            ValidateCtor(_count, listEntities, config);
            _config = config;
            _listEntities = listEntities;
            _pageStatsInOne = pageStartInOne;
            _defaultOrder = defaultOrder;
            _defaultSize = defaultSize;
            _defaultSort = defaultSort;
        }
        #endregion

        private void ValidateCtor(int count, IQueryable<E> listEntities, BaseConfigurePagination config)
        {
            if (count < 1 || config == null)
                throw new Exception($"The {(config != null ? nameof(listEntities) : nameof(config))} is empty!");
        }

        public IEnumerable<E> Content
        {
            get => Sort == "ASC" ? _listEntities.OrderBy(x => x.GetType().GetProperty(Order).GetValue(x, null)).Skip(Page * TotalElements).Take(Size).ToAsyncEnumerable().ToEnumerable()
            : _listEntities.OrderByDescending(x => x.GetType().GetProperty(Order).GetValue(x, null)).Skip(Page * TotalElements).Take(Size).ToAsyncEnumerable().ToEnumerable();
        }

        public int TotalElements
        {
            get => _count;
        }

        public string Sort
        {
            get => _config.sort ?? _defaultSort;
        }

        public string Order
        {
            get => _config.order ?? _defaultOrder;
        }

        public int Size
        {
            get => _config.size == 0 ? _defaultSize : _config.size;
        }

        public int Page
        {
            get => _pageStatsInOne ? _config.page - 1 : _config.page;
        }
    }
}