using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Saeed.Utilities.API.Requests.Parameters;
using Saeed.Utilities.API.Responses;
using Saeed.Utilities.Contracts;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Saeed.Utilities.Extensions.Collections
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// make paged list query, but without returning list yet. (call .ToList() | .ToListAsync() / .FirstOrDefault() / .SingleOrDefault(), after it or continue query as you want).
        /// </summary>
        /// <typeparam name="T">type of data</typeparam>
        /// <param name="queryable">the query</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">total item to take</param>
        /// <returns></returns>
        public static IQueryable<T> ToQueryablePagedList<T>(this IQueryable<T> queryable, int pageNumber = 1, int pageSize = 10)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
        /// <summary>
        /// return paged list based on specified page number and page size. (this method call .ToList() at the end)
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="queryable">the query</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">total item to take</param>
        /// <returns></returns>
        public static List<T> ToPagedList<T>(this IQueryable<T> queryable, int pageNumber = 1, int pageSize = 10)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public static Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            //var q = queryable
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize);
            // to fix monqodb queryable extension with ef iqueryable.
            return Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize), cancellationToken);
        }

        /// <summary>
        /// get a paged result with list of data and total number of entity records for paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(List<T> data, long Total)> ToPagedListWithTotalAsync<T>(this IQueryable<T> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            //var q = queryable
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize);
            // to fix monqodb queryable extension with ef iqueryable.
            return (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize), cancellationToken)
                .ConfigureAwait(false),
                Total: await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.LongCountAsync(queryable, cancellationToken)
                .ConfigureAwait(false));
        }


        /// <summary>
        /// return paged list based on specified page number and page size. (this method call .ToList() at the end)
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="queryable">the query</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">total item to take</param>
        /// <returns></returns>
        public static List<T> ToPagedList<T>(this IEnumerable<T> queryable, int pageNumber = 1, int pageSize = 10)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // mongo db helpers

        public static IMongoQueryable<T> ToQueryablePagedList<T>(this IMongoQueryable<T> queryable, int pageNumber = 1, int pageSize = 10)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
        public static Task<List<T>> ToPagedListAsync<T>(this IMongoQueryable<T> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// return a paged result with total, from Mongo Find Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<(List<T2>, long Total)> ToPagedListWithTotalAsync<T, T2>(this IFindFluent<T, T2> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return (await queryable
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken), await queryable.CountDocumentsAsync(cancellationToken));
        }

        /// <summary>
        /// return a message contract that contain list of data with pagedResponse Result and total records filled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MessageContract<List<T2>>> ToPagedMessageContractAsync<T, T2>(this IFindFluent<T, T2> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            long total = queryable.CountDocuments();
            return MessageContract.Ok(await queryable
                .Skip((pageNumber - 1) * pageSize) // skip amount of records based on page number and size and return nexts
                .Limit(pageSize) // take maximum records
                .ToListAsync(cancellationToken),
                new PagedResponse(pageNumber, pageSize, total));
        }
        /// <summary>
        /// return a paged result task from Mongo Find Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<List<T2>> ToPagedListAsync<T, T2>(this IFindFluent<T, T2> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return queryable
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public static async Task<MessageContract<List<T2>>> ToPagedMessageContractAsync<T, T2>(this IFindFluent<T, T2> queryable, RequestParameterWithSort filters, CancellationToken cancellationToken = default)
        {
            if (filters.SortOrderBy != null)
            {
                switch (filters.SortOrder)
                {
                    case Types.Enums.SortOrderTypes.Asc:
                        queryable = queryable.SortBy(x => filters.SortOrderBy);
                        break;
                    default:
                    case Types.Enums.SortOrderTypes.Desc:
                        queryable = queryable.SortByDescending(x => filters.SortOrderBy);
                        break;
                }
            }

            return MessageContract.Ok(await queryable
                         .Skip((filters.PageNumber - 1) * filters.PageSize)
                         .Limit(filters.PageSize)
                         .ToListAsync(cancellationToken),
                         filters.PageNumber, filters.PageSize, await queryable.CountDocumentsAsync(cancellationToken));

        }

        /// <summary>
        /// return a message contract result with list of T Data with paging and total records sets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MessageContract<List<T>>> ToPagedMessageContractAsync<T>(this IQueryable<T> queryable, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            //var q = queryable
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize);
            // to fix monqodb queryable extension with ef iqueryable.
            return MessageContract.Ok<List<T>>(data: await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
                cancellationToken).ConfigureAwait(false),
                pageNumber: pageNumber,
                pageSize: pageSize,
                total: await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .LongCountAsync(queryable, cancellationToken)
                .ConfigureAwait(false));
        }

        /// <summary>
        /// return a message contract result with sorted list of T Data (by filters.SortOrderBy), with paging and total records sets in request filters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MessageContract<List<T>>> ToPagedMessageContractAsync<T>(this IQueryable<T> queryable, RequestParameterWithSort filters, CancellationToken cancellationToken = default)
        {
            if (filters.SortOrderBy != null)
            {
                switch (filters.SortOrder)
                {
                    case Types.Enums.SortOrderTypes.Asc:
                        queryable = queryable.OrderBy(x => filters.SortOrderBy);
                        break;
                    default:
                    case Types.Enums.SortOrderTypes.Desc:
                        queryable = queryable.OrderByDescending(x => filters.SortOrderBy);
                        break;
                }
            }
            //var q = queryable
            //  .Skip((filters.PageNumber - 1) * filters.PageSize)
            //  .Take(filters.PageSize);

            // to fix monqodb queryable extension with ef iqueryable.
            return MessageContract.Ok<List<T>>(data: await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                queryable.Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize), cancellationToken)
                .ConfigureAwait(false),
                pageNumber: filters.PageNumber,
                pageSize: filters.PageSize,
                total: await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .LongCountAsync(queryable, cancellationToken)
                .ConfigureAwait(false));
        }

    }
}