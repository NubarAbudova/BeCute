using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;

namespace EnchantElegance.Persistence.Implementations.Repositories
{
	public class SliderRepository:Repository<Slider>,ISliderRepository
	{
        public SliderRepository(AppDbContext context) : base(context) { }
    }
}
