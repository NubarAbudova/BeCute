using System.Linq.Expressions;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Repositories
{
	public interface IRepository<T> where T : BaseEntity, new()
	{
		IQueryable<T> GetAll(bool isTracking = false, bool isIgnoreQuery = false, params string[] includes);
		IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
	  Expression<Func<T, object>>? orderExpression = null,
	  bool isDescending = false,
	  int skip = 0,
	  int take = 0,
	  bool isTracking = false,
	  bool isIgnoreQuery = false,
	  params string[] includes);

		Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);

		Task<T> GetByExpressionAsync(Expression<Func<T, bool>>? expression, bool isTracking = false, bool isIgnoreQuery = false, params string[] includes);
		Task<T> GetByIdAsync(int id, bool IgnoreQuery = false, bool IsTracking = false, params string[] includes);
		Task AddAsync(T entity);
		void Update(T entity);
		void Delete(T entity);
		void SoftDelete(T entity);
		void ReverseDelete(T entity);
		Task SaveChangesAsync();
	}
}
