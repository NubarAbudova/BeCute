using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;

namespace EnchantElegance.Persistence.Implementations.Repositories
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        public CategoryRepository(AppDbContext context):base(context)
        {
                
        }
    }
}
