using System.Linq;
using Models.BaseEnties.Pagination;

namespace Paginate
{
    /// <summary>
    /// Extension method to paginate entity E
    /// </summary>
    public static class Paginate
    {
        /// <summary>
        /// Paginate entity E, default values: pageStartInOne: false, sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity E</param>
        /// <param name="config">Config from pagination</param>
        /// <typeparam name="E">Entity E</typeparam>
        /// <returns>Paginated List E</returns>
        public static Pagination<E> PaginateTo<E>(this IQueryable<E> listEntities, BaseConfigurePagination config)
        where E : class => PaginateTo<E>(listEntities, config, false);

        /// <summary>
        /// Paginate entity E, default values: sort= ASC, order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity E</param>
        /// <param name="config">Config from pagination</param>
        /// <param name="pageStartInOne">If pagination starts on index 1</param>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static Pagination<E> PaginateTo<E>(this IQueryable<E> listEntities, BaseConfigurePagination config, bool pageStartInOne)
        where E : class => PaginateTo<E>(listEntities, config, pageStartInOne, "ASC");

        /// <summary>
        /// Paginate entity E, default values: order=Id, size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity E</param>
        /// <param name="config">Config from pagination</param>
        /// <param name="pageStartInOne">If pagination starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static Pagination<E> PaginateTo<E>(this IQueryable<E> listEntities, BaseConfigurePagination config, bool pageStartInOne, string defaultSort)
        where E : class => PaginateTo<E>(listEntities, config, pageStartInOne, defaultSort, "Id");

        /// <summary>
        /// Paginate entity E, default values: size=10
        /// </summary>
        /// <param name="listEntities">IQueryable from entity E</param>
        /// <param name="config">Config from pagination</param>
        /// <param name="pageStartInOne">If pagination starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static Pagination<E> PaginateTo<E>(this IQueryable<E> listEntities, BaseConfigurePagination config, bool pageStartInOne, string defaultSort, string defaultOrder)
        where E : class => PaginateTo<E>(listEntities, config, pageStartInOne, defaultSort, defaultOrder, 10);

        /// <summary>
        /// Paginate entity E, no default values
        /// </summary>
        /// <param name="listEntities">IQueryable from entity E</param>
        /// <param name="config">Config from pagination</param>
        /// <param name="pageStartInOne">If pagination starts on index 1</param>
        /// <param name="defaultSort">Default value to sort ("ASC" or "DESC")</param>
        /// <param name="defaultOrder">Default value to order (Name property)</param>>
        /// <param name="defaultSize">Default value to size</param>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static Pagination<E> PaginateTo<E>(this IQueryable<E> listEntities, BaseConfigurePagination config, bool pageStartInOne, string defaultSort, string defaultOrder, int defaultSize)
        where E : class => new Pagination<E>(listEntities, config, pageStartInOne, defaultSort, defaultOrder, defaultSize);
    }
}