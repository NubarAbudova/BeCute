using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;

namespace EnchantElegance.Persistence.Implementations.Repositories
{
	public class ColorRepository : Repository<Color>, IColorRepository
	{
		public ColorRepository(AppDbContext context) : base(context) { }
	}
}
