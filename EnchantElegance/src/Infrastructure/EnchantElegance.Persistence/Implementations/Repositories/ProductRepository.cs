﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
