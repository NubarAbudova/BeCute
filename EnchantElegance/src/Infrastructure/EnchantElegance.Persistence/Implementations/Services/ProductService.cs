using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;


namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;

		public ProductService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
		}

		public async Task<ItemVM<Product>> GetAllAsync(int page, int take)
		{
			List<Product> products = await _context.Products.ToListAsync();
			ItemVM<Product> categoryvm = new ItemVM<Product>
			{
				Items = products,
			};
			return categoryvm;
		}
		public async Task<List<string>> Create(ProductCreateDTO productCreateDTO)
		{
			List<string> str = new List<string>();
			if (productCreateDTO.Photo != null)
			{

				if (!productCreateDTO.Photo.ValidateType("image/"))
				{
					str.Add("File type does not match");
					return str;
				}
				if (!productCreateDTO.Photo.ValidateSize(2 * 1024))
				{
					str.Add("File size should not be larger than 2MB");
					return str;
				}
			}

			string fileName = await productCreateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
			


			Product product = new Product
			{
		
				Name = productCreateDTO.Name,
				Description= productCreateDTO.Description,
				CurrentPrice=productCreateDTO.CurrentPrice,
				OldPrice=productCreateDTO.OldPrice,
				CategoryId = (int)productCreateDTO.CategoryId,
				ProductColors = new List<ProductColor>(),
				ProductImages = new List<ProductImages>()


			};

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return str;
		}
		public async Task GetProductForUpdateAsync(int id)
		{
			Product product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

			if (product == null)
			{
				throw new Exception("Product is null");

			}

			ProductUpdateDTO updateDTO = _mapper.Map<ProductUpdateDTO>(product);

			await _context.SaveChangesAsync();
		}

		public async Task Update(int id, ProductUpdateDTO updateDTO)
		{
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
			{
				throw new Exception("Product is null");
			}


			product.Name = updateDTO.Name;

			//if (updateDTO.Photo != null)
			//{
			//	
			//	if (!updateDTO.Photo.ValidateType("image/"))
			//	{
			//		throw new Exception("File type does not match");
			//	}

			//	if (!updateDTO.Photo.ValidateSize(2 * 1024))
			//	{
			//		throw new Exception("File size should not be larger than 2MB");
			//	}

			//	
			//	if (!string.IsNullOrEmpty(Category.Image))
			//	{
			//		Category.Image.DeleteFile(_env.WebRootPath, "assets", "img");
			//	}

			//	// Yeni fotoğrafı ekleyin
			//	Category.Image = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
			//}

			await _context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			Product existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

			if (existed == null)
			{
				throw new Exception("Product is null");
			}

			try
			{
				_context.Products.Remove(existed);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw new Exception("Product is null");

			}

			//if (!string.IsNullOrEmpty(existed.Image))
			//{
			//	existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
			//}

			await _context.SaveChangesAsync();
		}

		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

	
	}
}
