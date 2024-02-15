using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;

namespace EnchantElegance.Persistence.Implementations.Repositories
{
	public class ProductRepository:Repository<Product>,IProductRepository
	{
        public ProductRepository(AppDbContext context) : base(context) { }
    }
}
