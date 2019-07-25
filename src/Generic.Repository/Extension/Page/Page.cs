using System;
using System.Collections.Generic;
using System.Linq;
using Generic.Repository.Cache;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;

namespace Generic.Repository.Extension.Page
{
    /// <summary>
    /// Extension method to paginate entity TValue
    /// </summary>
    public static class Page
    {

        #region Page<TValue>
        /// <summary>
        /// Paginate entity TValue, default values: pageStartInOne: false, sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <typeparam name="TValue">Entity E</typeparam>
        /// <returns>Paginated List E</returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config)
        where TValue : class => 
        ToPage(
            listEntities,
            cacheRepository,
            config, 
            false);

        /// <summary>
        /// Paginate entity TValue, default values: sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities, 
            ICacheRepository cacheRepository,
            IPageConfig config, 
            bool pageStartInOne)
        where TValue : class => 
        ToPage(
            listEntities, 
            cacheRepository,
            config, 
            pageStartInOne, 
            "ASC");

        /// <summary>
        /// Paginate entity TValue, default values: order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities, 
            ICacheRepository cacheRepository,
            IPageConfig config, 
            bool pageStartInOne, 
            string defaultSort)
        where TValue : class => 
        ToPage(
            listEntities,
            cacheRepository, 
            config, 
            pageStartInOne, 
            defaultSort, 
            "Id");

        /// <summary>
        /// Paginate entity TValue, default values: size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository, 
            IPageConfig config,
             bool pageStartInOne, 
             string defaultSort, 
             string defaultOrder)
        where TValue : class => 
        ToPage(
            listEntities, 
            cacheRepository,
            config, 
            pageStartInOne, 
            defaultSort, 
            defaultOrder, 
            10);

        /// <summary>
        /// Paginate entity TValue, no default values
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <param name="defaultSize">Default value to size</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config,
            bool pageStartInOne,
            string defaultSort,
            string defaultOrder,
            int defaultSize
            )
        where TValue : class =>
        new Page<TValue>(
            cacheRepository,
            listEntities,
            config,
            pageStartInOne,
            defaultSort,
            defaultOrder,
            defaultSize
        );
        #endregion

        #region Page<TValue, TResult>
        /// <summary>
        /// Paginate entity TValue mapping result to TResult by Func mapperTo,, default values: pageStartInOne: false, sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <typeparam name="TValue">Database type</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo,
            IPageConfig config)
        where TValue : class
        where TResult : class => 
        ToPage(
            listEntities,
            cacheRepository, 
            mapperTo, 
            config, 
            false);

        /// <summary>
        /// Paginate entity TValue mapping result to TResult by Func mapperTo, default values: sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <typeparam name="TValue">Database type</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <returns></returns>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo,
            IPageConfig config,
            bool pageStartInOne
            )
        where TValue : class
        where TResult : class =>
        ToPage(
            listEntities,
            cacheRepository,
            mapperTo,
            config,
            pageStartInOne,
            "ASC"
        );

        /// <summary>
        /// Paginate entity TValue mapping result to TResult by Func mapperTo, default values: order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <typeparam name="TValue">Database type</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <returns></returns>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo,
            IPageConfig config,
            bool pageStartInOne,
            string defaultSort)
        where TValue : class
        where TResult : class =>
        ToPage(
            listEntities,
            cacheRepository,
            mapperTo,
            config,
            pageStartInOne,
            defaultSort,
            "Id"
            );
        /// <summary>
        /// Paginate entity TValue mapping result to TResult by Func mapperTo, default values: size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <typeparam name="TValue">Database type</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <returns></returns>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities, 
            ICacheRepository cacheRepository,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo, 
            IPageConfig config, 
            bool pageStartInOne, 
            string defaultSort, 
            string defaultOrder)
        where TValue : class
        where TResult : class => 
        ToPage(
            listEntities,
            cacheRepository, 
            mapperTo, 
            config, 
            pageStartInOne, 
            defaultSort, 
            defaultOrder, 
            10);

        /// <summary>
        /// Paginate entity TValue mapping result to TResult by Func mapperTo, no default values
        /// </summary>
        /// <param name="listEntities">IQueryable from entity TValue</param>
        /// <param name="config">Config from Page</param>
        /// <param name="pageStartInOne">If Page starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <param name="defaultSize">Default value to size</param>
        /// <typeparam name="TValue">Database type</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <returns></returns>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository, 
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperTo, 
            IPageConfig config, 
            bool pageStartInOne, 
            string defaultSort, 
            string defaultOrder, 
            int defaultSize)
        where TValue : class
        where TResult : class => new Page<TValue, TResult>(
            cacheRepository,
            listEntities,
            mapperTo, 
            config, 
            pageStartInOne, 
            defaultSort, 
            defaultOrder, 
            defaultSize);
        #endregion

    }
}
