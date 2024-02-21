using System.Linq.Expressions;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;
        public Repository(AppDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }
        //GETALL
        public IQueryable<T> GetAll(bool isTracking = false, bool isIgnoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            if (isIgnoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();
            query = _addInclude(query, includes);
            return query;
        }


       

        //GETALLWHERE
        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderExpression = null, bool isDescending = false, int skip = 0, int take = 0, bool isTracking = false, bool isIgnoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (orderExpression != null)
            {
                if (isDescending) query = query.OrderByDescending(orderExpression);
                else query = query.OrderBy(orderExpression);
            }

            if (skip != 0) query = query.Skip(skip);
            if (take != 0) query = query.Take(take);


            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }

            if (isIgnoreQuery) query = query.IgnoreQueryFilters();
            return isTracking ? query : query.AsNoTracking();
        }
		public IQueryable<T> GetOrderBy(Expression<Func<T, object>>? orderExpression = null, bool IsDescending = false)
		{
			IQueryable<T> query = _context.Set<T>();
			if (orderExpression != null)
			{
				if (IsDescending)
				{
					query = query.OrderByDescending(orderExpression);
				}
				else
				{
					query = query.OrderBy(orderExpression);
				}

			}
			return query;
		}
		public IQueryable<T> GetPagination(int skip = 0, int take = 0, bool IgnoreQuery = true, Expression<Func<T, object>>? orderExpression = null, bool IsDescending = false, params string[] includes)
		{
			IQueryable<T> query = _context.Set<T>();
			if (skip != 0) query = query.Skip(skip);
			if (take != 0) query = query.Take(take);
			if (orderExpression != null)
			{
				if (IsDescending)
				{
					query = query.OrderByDescending(orderExpression);
				}
				else
				{
					query = query.OrderBy(orderExpression);
				}

			}
			query = _addIncludes(query, includes);
			if (IgnoreQuery)
			{
				query = query.IgnoreQueryFilters();
			}
			return query;
		}
		private IQueryable<T> _addIncludes(IQueryable<T> query, params string[] includes)
		{
			if (includes != null)
			{
				for (int i = 0; i < includes.Length; i++)
				{
					query = query.Include(includes[i]);
				}
			}
			return query;
		}
		//ADDASYNC
		public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);

        }
        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.AnyAsync(expression);
        }

        //GETBYIDASYNC
        public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool isIgnoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(x => x.Id == id);
            if (isIgnoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();

            query = _addInclude(query, includes);
            return await query.FirstOrDefaultAsync();
        }

         public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool? isDeleted = null, params string[] includes)
        {
            IQueryable<T> query = _table.Where(x => x.Id == id);
            if (isDeleted == null) query = query.Where(x => x.IsDeleted == null);
            else if (isDeleted == false) query = query.Where(x => x.IsDeleted == false);
            else if (isDeleted == true) query = query.Where(x => x.IsDeleted == true);
            if (!isTracking) query = query.AsNoTracking();
            if (includes != null) query = _addIncludes(query, includes);
            return await query.FirstOrDefaultAsync();
        }

        //GETBYEXPRESSION

        public Task<T> GetByExpressionAsync(Expression<Func<T, bool>>? expression, bool isTracking = false, bool isIgnoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(expression);
            if (isIgnoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();

            query = _addInclude(query, includes);

            return query.FirstOrDefaultAsync();
        }

        //UPDATE
        public void Update(T entity)
        {
            _table.Update(entity);
        }

        //DELETES
        public void Delete(T entity)
        {
            _table.Remove(entity);
        }
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }
        public void ReverseDelete(T entity)
        {
            entity.IsDeleted = false;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private IQueryable<T> _addInclude(IQueryable<T> query, params string[] includes)
        {
            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }
            return query;
        }
    }
}
