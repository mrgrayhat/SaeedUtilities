using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Saeed.Utilities.API.Requests.Parameters;
using Saeed.Utilities.Types.Enums;

using Microsoft.EntityFrameworkCore;

using Saeed.Utilities.Gaurds;

namespace Saeed.Utilities.Infrastructures.Ef.Repositories
{
    /// <summary>
    /// an generic repository with top level features useful for most repositories.
    /// repositories must inherit from this base, to get all Crud Base and custom repository features
    /// </summary>
    /// <typeparam name="TEntity">entity</typeparam>
    /// <typeparam name="TKey">primary key</typeparam>
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// the DbContext Instance.
        /// </summary>
        public DbContext _context { get; init; }

        #region Repository Infra

        /// <summary>
        /// disable entity tracking is a performance best practice for read-only scenarios. tracking increase memory allocation and decrease query performance. if you don't need to modify/delete returned entity, disable it.
        /// </summary>
        public bool NoTracking { get; set; } = false;
        /// <summary>
        /// splitting query behavior is a performance best practice for queries which load multiple relations / big queries. but not guarantee data consistency cause to multiple tables call. default behavior is AsSingleQuery which load all query and tables in single call. but with a larger query and higher load.
        /// </summary>
        public bool SplitQuery { get; set; } = false;
        /// <summary>
        /// ignore every global filter which is applied to db context (like soft delete, types ...)
        /// </summary>
        public bool IgnoreGlobalFilter { get; set; } = false;

        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool IsPagingEnabled { get; set; } = false;
        //public Func<IEnumerable<TEntity>, IEnumerable<TEntity>> PostProcessingAction { get; set; }
        public bool CacheEnabled { get; set; } = true;
        public string CacheKey { get; set; }
        public SortOrderTypes? SortOrder { get; set; }
        public string SortOrderBy { get; set; } = null;
        //public string SearchTerm { get; set; }

        #endregion

        public IGenericRepository<TEntity, TKey> AsPaging(RequestParameterBase filters)
        {
            Take = filters.PageSize;
            filters.PageSize = 100;
            Skip = (filters.PageNumber - 1) * filters.PageSize;
            return this;
        }
        public IGenericRepository<TEntity, TKey> AsPaging(int pageNumber, int pageSize)
        {
            Take = pageSize;
            Skip = (pageNumber - 1) * pageSize;
            return this;
        }
        public IGenericRepository<TEntity, TKey> AsSorting(RequestParameterWithSort filters)
        {
            SortOrder = filters.SortOrder;
            SortOrderBy = filters.SortOrderBy;
            return this;
        }
        public IGenericRepository<TEntity, TKey> AsSorting(SortOrderTypes sortOrderType, string sortOrderBy = null)
        {
            SortOrder = sortOrderType;
            SortOrderBy = sortOrderBy;
            return this;
        }
        //public IGenericRepository<TEntity, TKey> WithSearch(string term)
        //{
        //    SearchBy = term;
        //    return this;
        //}
        public GenericRepository(DbContext dbContext) //: base(dbContext)
        {
            _context = Guard.Against.Null(dbContext, nameof(dbContext));
            //_entity = this._context.Set<TEntity>();
        }
        #region Repository Infra Operators

        /// <summary>
        /// set global filter ignore to true. <seealso cref="IgnoreGlobalFilter"/>
        /// </summary>
        public IGenericRepository<TEntity, TKey> IgnoreGlobalFilters()
        {
            IgnoreGlobalFilter = true;
            return this;
        }
        /// <summary>
        /// set global filter ignore to false (default). <seealso cref="IgnoreGlobalFilter"/>
        /// </summary>
        public IGenericRepository<TEntity, TKey> IncludeGlobalFilter()
        {
            IgnoreGlobalFilter = false;
            return this;
        }
        /// <summary>
        /// set AsNoTracking to true. <see cref="NoTracking"/>
        /// </summary>
        public IGenericRepository<TEntity, TKey> AsNoTracking()
        {
            NoTracking = true;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return this;
        }
        /// <summary>
        /// set AsNoTracking to false (default). <see cref="NoTracking"/>
        /// 
        /// </summary>
        public IGenericRepository<TEntity, TKey> AsTracking()
        {
            NoTracking = false;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return this;
        }
        /// <summary>
        /// set query behavior to split query. <see cref="SplitQuery"/>
        /// </summary>
        public IGenericRepository<TEntity, TKey> AsSplitQuery()
        {
            SplitQuery = true;
            return this;
        }
        /// <summary>
        /// set query behavior to single query (default). <see cref="SplitQuery"/>
        /// </summary>
        public IGenericRepository<TEntity, TKey> AsSingleQuery()
        {
            SplitQuery = false;
            return this;
        }

        #endregion
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(entity, nameof(entity));
            var added = await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            //await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return added.Entity;
        }
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Guard.Against.NullOrEmpty(entities, nameof(entities));
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            //await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task DeleteAsync(TKey entityKey, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>();
            var find = await dbSet.FindAsync(new object[] { entityKey }, cancellationToken: cancellationToken);
            Guard.Against.NotFound(nameof(entityKey), find, nameof(entityKey));
            // TODO: is soft delete able
            //if (IgnoreGlobalFilter)
            //    dbSet.IgnoreQueryFilters<TEntity>();
            dbSet.Remove(find);
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(expression, nameof(expression));
            var dbSet = _context.Set<TEntity>();
            var find = await dbSet
                .Where(expression)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            // TODO: is soft delete able
            dbSet.Remove(find);
            //return await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void Delete(TEntity entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            //if (NoTracking)
            //    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var dbSet = _context.Set<TEntity>();
            // TODO: is soft delete able
            dbSet.Remove(entity);
            //return await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            Guard.Against.NullOrEmpty(entities, nameof(entities));
            // TODO: is soft delete able

            _context.Set<TEntity>().RemoveRange(entities);
            //await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey entityKey, CancellationToken cancellationToken = default)
        {
            if (NoTracking)
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await _context.Set<TEntity>().FindAsync(new object[] { entityKey }, cancellationToken: cancellationToken);
        }
        public string BuildCacheKey(string entityName, TKey entityKey)
        {
            return string.Concat(entityName.ToLower(), ":", entityKey.ToString());
        }
        public string BuildCacheKey(ref TEntity entity, TKey entityKey)
        {
            return string.Concat(GetTypeName(entity), ":", entityKey.ToString());
        }
        public string BuildCacheKey(ref TEntity entity, TKey entityKey, string[] extraArgs)
        {
            return string.Join(separator: ':', GetTypeName(entity), entityKey.ToString(), extraArgs);
        }

        public string BuildCacheKeyFromExpression(ref List<Expression<Func<TEntity, bool>>> expressions)
        {
            return expressions.Any() ?
                string.Join(separator: ':',
                //expressions[0].ReturnType.Name ,
                expressions.ConvertAll(x => x.Body.ToString().Trim('(', ')', '"')))
                : null; // make an short string of converted expressions
        }
        public string BuildCacheKeyFromExpression(ref List<Expression<Func<TEntity, bool>>> expressions, int pageNumber, int pageSize, SortOrderTypes sortOrder = SortOrderTypes.Desc)
        {
            return expressions.Any() ?
                string.Join(separator: ':', expressions.ConvertAll(x => x.Body.ToString().Trim('(', ')', '"')),
                    pageNumber.ToString(), pageSize.ToString(), sortOrder.ToString())
                : null; // make an short string of converted expressions
        }

        //public virtual Task<TEntity> GetByIdAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<IncludeExpressionInfo> includeExpressions, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException("this method isn't implemented yet");
        //}

        //public virtual async Task<TEntity> GetByIdAsync(TKey entityKey, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default)
        //{
        //    var dbSet = _context.Set<TEntity>().AsQueryable<TEntity>();
        //    dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
        //    dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;
        //    dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
        //    foreach (var exp in includeExpressions)
        //    {
        //        dbSet = dbSet.Include(exp);
        //    }

        //    return await dbSet.FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        //}

        public virtual async Task<TEntity> GetSingleAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable<TEntity>();
            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            foreach (var exp in whereExpressions)
            {
                dbSet = dbSet.Where(exp);
            }
            return await dbSet.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> GetFirstAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable<TEntity>();
            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            foreach (var exp in whereExpressions)
            {
                dbSet = dbSet.Where(exp);
            }
            return await dbSet.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        //public virtual async Task<TEntity> GetByIdAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default)
        //{
        //    var dbSet = _context.Set<TEntity>().AsQueryable<TEntity>();
        //    dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
        //    dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;
        //    dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();

        //    foreach (var nav in includeExpressions)
        //    {
        //        dbSet = dbSet.Include(nav);
        //    }
        //    foreach (var exp in whereExpressions)
        //    {
        //        dbSet = dbSet.Where(exp);
        //    }
        //    return await dbSet.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        //}

        public virtual async Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();

            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

            // paging
            if (Skip.HasValue)
                dbSet = dbSet.Skip(Skip.Value);
            if (Take.HasValue)
                dbSet = dbSet.Take(Take.Value);

            return await dbSet
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();
            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

            if (whereExpressions != null)
                foreach (var where in whereExpressions)
                {
                    dbSet = dbSet.Where(where);
                }

            // paging
            if (Skip.HasValue)
                dbSet = dbSet.Skip(Skip.Value);
            if (Take.HasValue)
                dbSet = dbSet.Take(Take.Value);

            return await dbSet
                .ToListAsync(cancellationToken).ConfigureAwait(false);

        }

        //public virtual async Task<IReadOnlyCollection<TEntity>> ListAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default)
        //{
        //    var dbSet = _context.Set<TEntity>().AsQueryable();
        //    dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
        //    dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
        //    dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

        //    foreach (var nav in includeExpressions)
        //    {
        //        dbSet = dbSet.Include(nav);
        //    }
        //    foreach (var exp in whereExpressions)
        //    {
        //        dbSet = dbSet.Where(exp);
        //    }

        //    // paging
        //    if (Skip.HasValue)
        //        dbSet = dbSet.Skip(Skip.Value);
        //    if (Take.HasValue)
        //        dbSet = dbSet.Take(Take.Value);

        //    return await dbSet.ToListAsync(cancellationToken).ConfigureAwait(false);

        public virtual TEntity Update(TEntity entity)
        {
            return _context.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }
        /// <summary>
        /// update and call SaveChanges
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var updated = _context.Update(entity);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return updated.Entity;
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();
            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

            return await dbSet.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        }
        public virtual async Task<int> CountAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();
            dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
            dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

            foreach (var exp in whereExpressions)
            {
                dbSet = dbSet.Where(exp);
            }

            return await dbSet.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        //public virtual async Task<long> CountAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, IEnumerable<Expression<Func<TEntity, TKey>>> includeExpressions, CancellationToken cancellationToken = default)
        //{
        //    var dbSet = _context.Set<TEntity>().AsQueryable();
        //    dbSet = NoTracking ? dbSet.AsNoTracking() : dbSet.AsTracking(QueryTrackingBehavior.TrackAll);
        //    dbSet = SplitQuery ? dbSet.AsSplitQuery() : dbSet.AsSingleQuery();
        //    dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;

        //    foreach (var nav in includeExpressions)
        //    {
        //        dbSet = dbSet.Include(nav);
        //    }
        //    foreach (var exp in whereExpressions)
        //    {
        //        dbSet = dbSet.Where(exp);
        //    }

        //    return await dbSet.LongCountAsync(cancellationToken).ConfigureAwait(false);
        //}
        public virtual async Task<bool> IsUniqueAsync(IEnumerable<Expression<Func<TEntity, bool>>> whereExpressions, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>().AsNoTracking();
            dbSet = IgnoreGlobalFilter ? dbSet.IgnoreQueryFilters() : dbSet;
            foreach (var exp in whereExpressions)
            {
                dbSet = dbSet.Where(exp);
            }

            return await dbSet.AnyAsync(cancellationToken).ConfigureAwait(false);
        }


        public virtual bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
        public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
        }

        private string GetTypeName(in TEntity entity)
        {
            return entity.GetType().Name.ToLower();
        }
    }
}