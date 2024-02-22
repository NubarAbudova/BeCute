using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace EnchantElegance.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? search, int? order, int? categoryId, int page = 1, int take = 3)
        {
            IQueryable<Domain.Entities.Product> query = _context.Products.Include(p => p.ProductImages).AsQueryable();

            switch (order)
            {
                case 1:
                    query = query.OrderBy(p => p.CurrentPrice);
                    break;
                case 2:
                    query = query.OrderBy(p => p.Category.Name);
                    break;
                case 3:
                    query = query.OrderBy(p => p.Name);
                    break;
                case 4:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            if (categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / take);

            query = query.Skip((page - 1) * take).Take(take);

            ShopDTO shopDTO = new ShopDTO()
            {
                Categories = await _context.Categories.Include(c => c.Products).ToListAsync(),
                Products = await query.ToListAsync(),
                CategoryId = categoryId,
                Order = order,
                Search = search,

                 Pagination= new PaginationVM<Domain.Entities.Product>
                {
                     CurrentPage = page,
                     TotalPage = totalPages,
                     Items = query.ToList()
                 }
            };

            return View(shopDTO);
        }
    }
}