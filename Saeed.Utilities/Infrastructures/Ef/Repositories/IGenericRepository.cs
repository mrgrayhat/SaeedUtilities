using Saeed.Utilities.API.Requests.Parameters;
using Saeed.Utilities.Types.Enums;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Saeed.Utilities.Infrastructures.Ef.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">entity</typeparam>
    /// <typeparam name="TKey">entity key</typeparam>
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        #region Repository Infra
        public DbContext _context { get; init; }
        /// <summary>
        /// disable entity tracking is a performance best practice for read-only scenarios. tracking increase memory allocation and decrease query performance. if you don't need to modify/delete returned entity, disable it.
        /// </summary>
        public bool NoTracking { get; set; }
        /// <summary>
        /// splitting query behavior is a performance best practice for queries which load multiple relations / big queries. but not guarantee data consistency cause to multiple tables call. default behavior is AsSingleQuery which load all query and tables in single call. but with a larger query and higher load.
        /// </summary>
        public bool SplitQuery { get; set; }
        /// <summary>
        /// ignore every global filter which is applied to db context (like soft delete, types ...)
        /// </summary>
        public bool IgnoreGlobalFilter { get; set; }

        //public string SearchTerm { get; set; }
        public string SortOrderBy { get; set; }
        public SortOrderTypes? SortOrder { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool CacheEnabled { get; set; }
        public string CacheKey { get; set; }

        public abstract IGenericRepository<TEntity, TKey> AsSorting(RequestParameterWithSort filters);
        public abstract IGenericRepository<TEntity, TKey> AsSorting(SortOrderTypes sortOrderType, string sortOrderBy = null);
        public abstract IGenericRepository<TEntity, TKey> AsPaging(RequestParameterBase filters);
        public abstract IGenericRepository<TEntity, TKey> AsPaging(int pageNumber, int pageSize);
        /// <summary>
        /// set global filter ignore to true. <seealso cref="IgnoreGlobalFilter"/>
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> IgnoreGlobalFilters();

        /// <summary>
        /// set global filter ignore to false (default). <seealso cref="IgnoreGlobalFilter"/>
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> IncludeGlobalFilter();

        /// <summary>
        /// set AsNoTracking to true. <see cref="NoTracking"/>
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> AsNoTracking();

        /// <summary>
        /// set AsNoTracking to false (default). <see cref="NoTracking"/>
        /// 
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> AsTracking();

        /// <summary>
        /// set query behavior to split query. <see cref="SplitQuery"/>
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> AsSplitQuery();

        /// <summary>
        /// set query behavior to single query (default). <see cref="SplitQuery"/>
        /// </summary>
        public abstract IGenericRepository<TEntity, TKey> AsSingleQuery();

        public abstract string BuildCacheKey(string entityName, TKey entityKey);
        public abstract string BuildCacheKey(ref TEntity entity, TKey entityKey);
        public abstract string BuildCacheKey(ref TEntity entity, TKey entityKey, string[] extraArgs);

        public abstract string BuildCacheKeyFromExpression(ref List<Expression<Func<TEntity, bool>>> expressions);
        public abstract string BuildCacheKeyFromExpression(ref List<Expression<Func<TEntity, bool>>> expressions, int pageNumber, int pageSize, SortOrderTypes sortOrder = SortOrderTypes.Desc);
        #endregion

        #region CRUD BASE

        public abstract Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        public abstract Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        public abstract Task DeleteAsync(TKey entityKey, CancellationToken cancellationToken = default);
        public abstract void Delete(TEntity entity);
        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        public abstract void DeleteRange(IEnumerable<TEntity> entities);

        public abstract Task<TEntity> GetByIdAsync(TKey entityKey, CancellationToken cancellationToken = default);
        //public abstract Task<TEntity> GetByIdAsync(TKey entityKey, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default);
        public abstract Task<TEntity> GetSingleAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default);
        public abstract Task<TEntity> GetFirstAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default);
        //public abstract Task<TEntity> GetByIdAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default);
        //public abstract Task<TEntity> GetByIdAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<IncludeExpressionInfo> includeExpressions, CancellationToken cancellationToken = default);

        public abstract Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default);
        public abstract Task<IReadOnlyCollection<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default);
        //public abstract Task<IReadOnlyCollection<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default);

        public abstract TEntity Update(TEntity entity);
        public abstract void UpdateRange(IEnumerable<TEntity> entities);
        public abstract Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        public abstract Task<int> CountAsync(CancellationToken cancellationToken = default);
        public abstract Task<int> CountAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default);
        //public abstract Task<long> CountAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default);

        public abstract Task<bool> IsUniqueAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default);


        public abstract bool SaveChanges();
        public abstract Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        #endregion

    }
}