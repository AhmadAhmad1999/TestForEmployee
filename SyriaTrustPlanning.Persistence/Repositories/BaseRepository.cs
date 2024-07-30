using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyriaTrustPlanning.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected SyriaTrustPlanningDbContext _dbContext { get; set; }
        protected readonly DbSet<T> _DbSet;
        public BaseRepository(SyriaTrustPlanningDbContext dbContext)
        {
            _dbContext = dbContext;
            _DbSet = _dbContext.Set<T>();
        }

        public virtual int GetCount(Expression<Func<T, bool>>? predicate)
        {
            if (predicate == null)
                return _DbSet.Count();

            return _DbSet.AsNoTracking().Where(predicate).Count();
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate)
        {
            if (predicate == null)
                return await _DbSet.CountAsync();

            return await _DbSet.AsNoTracking().Where(predicate).CountAsync();
        }
        public virtual async Task<T?> GetByIdAsync(int? id)
        {
            T? t = await _DbSet.FindAsync(id);
            return t;
        }
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            T? t = await _DbSet.FindAsync(id);
            return t;
        }
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _DbSet.AsNoTracking().ToListAsync();
        }
        public IEnumerable<T> AsEnumerable()
        {
            return _DbSet.AsNoTracking().AsEnumerable();
        }
        public async virtual Task<IReadOnlyList<T>> GetPagedReponseWithPredicateAsync(Expression<Func<T, bool>>? predicate, int page, int size)
        {
            if (size == -1 || page == 0)
                return await _DbSet.AsNoTracking().Where(predicate!).ToListAsync();
            if (size == 0)
                size = 10;
            return await _DbSet.AsNoTracking().Where(predicate!).Skip((page - 1) * size).Take(size).ToListAsync();
        }
        public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        {
            if (size == -1 || page == 0)
                return await _DbSet.AsNoTracking().ToListAsync();
            if (size == 0)
                size = 10;
            return await _DbSet.AsNoTracking().Skip((page - 1) * size).Take(size).ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _DbSet.AddAsync(entity);
            typeof(T).GetProperty("CreatedAt")!.SetValue(entity, DateTime.UtcNow);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _DbSet.AddRangeAsync(entities);

            PropertyInfo createdAtProperty = typeof(T).GetProperty("CreatedAt")!;
            if (createdAtProperty != null && createdAtProperty.PropertyType == typeof(DateTime))
            {
                DateTime currentTime = DateTime.Now;
                foreach (T entity in entities)
                {
                    createdAtProperty.SetValue(entity, currentTime);
                }
            }

            await _dbContext.SaveChangesAsync();

            return entities;
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            typeof(T).GetProperty("isDeleted")!.SetValue(entity, true);
            typeof(T).GetProperty("DeletedAt")!.SetValue(entity, DateTime.UtcNow);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                typeof(T).GetProperty("isDeleted")!.SetValue(entity, true);
                typeof(T).GetProperty("DeletedAt")!.SetValue(entity, DateTime.UtcNow);
                _dbContext.Entry(entity).State = EntityState.Modified;
            });

            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveListAsync(IEnumerable<T> entities)
        {
            _dbContext.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _DbSet.AsNoTracking().Where(predicate);
        }
        public IQueryable<T> WhereThenFilter(Expression<Func<T, bool>> predicate, FilterObject filterObject)
        {
            IQueryable<T> query = _DbSet.AsNoTracking().Where(predicate);

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject);
            }

            return query;
        }
        public async virtual Task<IReadOnlyList<T>> GetFilterThenPagedReponseAsync(FilterObject filterObject, int page, int size)
        {
            var query = new List<T>();

            if (size == -1 || page == 0)
            {
                query = await _DbSet.AsNoTracking().ToListAsync();
            }

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject).ToList();
            }
            if (size != -1)
            {
                if (size == 0)
                    size = 10;
                query = query
                    .Skip((page - 1) * size).Take(size).ToList();
            }

            return query;
        }
        public async virtual Task<IReadOnlyList<T>> GetWhereThenPagedReponseAsync(Expression<Func<T, bool>> predicate, FilterObject filterObject, int page, int size)
        {
            var query = await _DbSet.AsNoTracking().Where(predicate).ToListAsync();

            if (size == -1 || page == 0)
            {
                query = await _DbSet.AsNoTracking().Where(predicate).ToListAsync();
            }

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject).ToList();
            }

            if (size == 0)
                size = 10;
            query = query
                .Skip((page - 1) * size).Take(size).ToList();

            return query;
        }
        public async virtual Task<IReadOnlyList<T>> GetWhereThenPagedReponseAsync(Expression<Func<T, bool>> predicate, int page, int size)
        {
            if (size == -1 || page == 0)
                return await _DbSet.AsNoTracking().Where(predicate).ToListAsync();
            if (size == 0)
                size = 10;
            return await _DbSet.AsNoTracking()
                .Where(predicate).Skip((page - 1) * size).Take(size).ToListAsync();
        }
        public T? FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _DbSet.AsNoTracking().FirstOrDefault(predicate);
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public T? LastOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _DbSet.AsNoTracking().LastOrDefault(predicate);
        }
        public async Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _DbSet.AsNoTracking().LastOrDefaultAsync(predicate);
        }
        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _DbSet.AsNoTracking().Select(selector);
        }
        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _DbSet.AsNoTracking().OrderBy(keySelector);
        }
        public IQueryable<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector, int page, int size)
        {
            if (size == -1 || page == 0)
                return _DbSet.AsNoTracking().OrderByDescending(keySelector);
            if (size == 0)
                size = 10;
            return _DbSet.AsNoTracking().OrderByDescending(keySelector).Skip((page - 1) * size).Take(size);
        }
        public IQueryable<T> OrderByDescending<TKey>(FilterObject filterObject, Expression<Func<T, TKey>> keySelector, int page, int size)
        {
            IQueryable<T> query = _DbSet.AsNoTracking();

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject);
            }

            if (size == -1 || page == 0)
                return _DbSet.AsNoTracking().OrderByDescending(keySelector);
            if (size == 0)
            {
                size = 10;
                query.AsNoTracking().OrderByDescending(keySelector).Skip((page - 1) * size).Take(size);
            }
            else
            {
                query.AsNoTracking().OrderByDescending(keySelector).Skip((page - 1) * size).Take(size);
            }

            return query;
        }
        public IQueryable<T> Include(string navigationPropertyPath)
        {
            return _DbSet.AsNoTracking().Include(navigationPropertyPath);
        }
        public IQueryable<T> Include(Expression<Func<T, object>> navigationProperty)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            return _DbSet.AsNoTracking().Include(navigationPropertyPath);
        }
        public IQueryable<T> Include(Expression<Func<T, object>> navigationProperty, FilterObject filterObject)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            var query = _DbSet.AsNoTracking().Include(navigationPropertyPath);

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject);
            }
            return query;
        }
        public IQueryable<T> IncludeThenWhere(Expression<Func<T, object>> navigationProperty,
            Expression<Func<T, bool>> predicate)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            return _DbSet.AsNoTracking().Include(navigationPropertyPath).Where(predicate);
        }
        public IQueryable<T> WhereThenIncludeThenPagination(
            Expression<Func<T, bool>> predicate, int page, int size,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> query;

            if (size == 0)
                size = 10;

            if (size == -1 || page == 0)
                query = _DbSet.AsNoTracking().Where(predicate);
            else
                query = _DbSet.AsNoTracking()
                    .Where(predicate).Skip((page - 1) * size).Take(size);

            foreach (var navigationProperty in navigationProperties)
            {
                string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
                query = query.Include(navigationPropertyPath);
            }

            return query;
        }
        public IQueryable<T> WhereThenInclude(Expression<Func<T, bool>> predicate, FilterObject filterObject, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> query = _DbSet.AsNoTracking().Where(predicate);

            if (filterObject != null && filterObject.Filters != null)
            {
                query = Filtration(filterObject);
            }

            foreach (var navigationProperty in navigationProperties)
            {
                string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
                query = query.Include(navigationPropertyPath);
            }

            return query;
        }

        public IQueryable<T> WhereThenInclude(
           Expression<Func<T, bool>> predicate,
           params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> query = _DbSet.AsNoTracking().Where(predicate);

            foreach (var navigationProperty in navigationProperties)
            {
                string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
                query = query.Include(navigationPropertyPath);
            }

            return query;
        }
        public T? IncludeThenFirstOrDefault(Expression<Func<T, object>> navigationProperty,
            Expression<Func<T, bool>> predicate)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            return _DbSet.AsNoTracking().Include(navigationPropertyPath).FirstOrDefault(predicate);
        }
        public async Task<T?> IncludeThenFirstOrDefaultAsync(Expression<Func<T, object>> navigationProperty,
            Expression<Func<T, bool>> predicate)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            return await _DbSet.AsNoTracking().Include(navigationPropertyPath).FirstOrDefaultAsync(predicate);
        }
        public T? IncludeThenLastOrDefault(Expression<Func<T, object>> navigationProperty,
            Expression<Func<T, bool>> predicate)
        {
            string navigationPropertyPath = GetNavigationPropertyPath(navigationProperty);
            return _DbSet.AsNoTracking().Include(navigationPropertyPath).LastOrDefault(predicate);
        }
        private string GetNavigationPropertyPath(Expression<Func<T, object>> navigationProperty)
        {
            if (navigationProperty == null)
                throw new ArgumentNullException(nameof(navigationProperty));

            if (!(navigationProperty.Body is MemberExpression memberExpression))
                throw new ArgumentException("Invalid expression type. Expected MemberExpression.", nameof(navigationProperty));

            var properties = memberExpression.ToString().Split('.');
            return string.Join(".", properties.Skip(1));
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> ExecuteUpdateAsync(string sql)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }


        public IQueryable<T> Filtration(FilterObject filterObject)
        {
            IQueryable<T> query = _DbSet.AsNoTracking();

            foreach (var filter in filterObject.Filters!)
            {
                var propertyType = typeof(T).GetProperty(filter.Key!)?.PropertyType;

                if (propertyType != null)
                {
                    if (filter.Value == null)
                    {
                        // Process null value
                        query = query.Where(entity => EF.Property<object>(entity, filter.Key!) == null);
                    }
                    else if (propertyType == typeof(string) && filter.Value is string stringValue)
                    {
                        // Process string value
                        if (!string.IsNullOrEmpty(stringValue))
                        {
                            query = query.Where(entity => EF.Property<string>(entity, filter.Key!).Contains(stringValue));
                        }
                    }
                    else if (propertyType == typeof(int) && filter.Value is string IntValue)
                    {
                        int Value = int.Parse(IntValue);
                        // Process string value
                        if (!string.IsNullOrEmpty(IntValue))
                        {
                            query = query.Where(entity => EF.Property<int>(entity, filter.Key!) == Value);
                        }
                    }
                    else if (propertyType == typeof(bool) && filter.Value is string boolValue)
                    {
                        bool Value = bool.Parse(boolValue);

                        query = query.Where(entity => EF.Property<bool>(entity, filter.Key!) == Value);
                    }

                    else if (propertyType == typeof(DateTime) && filter.DateRange is DateTimeRange dateRange)
                    {
                        // Process date range
                        if (dateRange.StartDate != null && dateRange.EndDate != null)
                        {
                            query = query.Where(entity =>
                                EF.Property<DateTime>(entity, filter.Key) >= dateRange.StartDate &&
                                EF.Property<DateTime>(entity, filter.Key) <= dateRange.EndDate);
                        }
                        else if (dateRange.StartDate != null)
                        {
                            query = query.Where(entity =>
                                EF.Property<DateTime>(entity, filter.Key) >= dateRange.StartDate);
                        }
                        else if (dateRange.EndDate != null)
                        {
                            query = query.Where(entity =>
                                EF.Property<DateTime>(entity, filter.Key) <= dateRange.EndDate);
                        }
                    }
                    else
                    {
                        // Process other value types
                        query = query.Where(entity => EF.Property<object>(entity, filter.Key!).Equals(filter.Value));
                    }
                }
            }
            return query;
        }
        public async Task<List<string>> GetPropertyNames()
        {
            List<string> PropertyNames = new List<string>();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (!propertyInfo.GetModifiedPropertyType().IsListType())
                    PropertyNames.Add(propertyInfo.Name);
            }
            return PropertyNames;
        }
    }
}
