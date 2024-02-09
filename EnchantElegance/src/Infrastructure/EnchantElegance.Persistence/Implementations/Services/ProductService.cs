using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

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
			List<Product> products = await _context.Products
				.Include(p=>p.Category)
				.Include(p=>p.ProductImages.Where(pi=>pi.IsPrimary==true))
				.ToListAsync();
			ItemVM<Product> categoryvm = new ItemVM<Product>
			{
				Items = products,
			};
			return categoryvm;
		}
		public ProductCreateDTO GetProductCreateDTO()
		{
			var productCreateDTO = new ProductCreateDTO();

			// Kategori listesini doldurun
			productCreateDTO.CategoryList = GetCategoryList();

			return productCreateDTO;
		}

		private List<SelectListItem> GetCategoryList()
		{
			List<Category> categories = _context.Categories.ToList(); // Gerçek kategori verilerini veritabanından al

			// Kategori listesini SelectListItem'lar listesine çevir
			List<SelectListItem> categoryList = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();

			return categoryList;
		}
		public async Task<List<string>> Create(ProductCreateDTO productCreateDTO)
		{
			List<string> errors = new List<string>();

			if (string.IsNullOrEmpty(productCreateDTO.Name))
			{
				errors.Add("Product name is required.");
			}
		
			if (productCreateDTO.CurrentPrice <= 0)
			{
				errors.Add("Product price should be greater than zero.");
			}
		
			if (string.IsNullOrEmpty(productCreateDTO.Description))
			{
				errors.Add("Product description is required.");
			}

			if (productCreateDTO.Photo != null)
			{
			
				if (!productCreateDTO.Photo.ValidateType("image/"))
				{
					errors.Add("File type does not match. Please upload a valid image.");
				}

				if (!productCreateDTO.Photo.ValidateSize(2 * 1024))
				{
					errors.Add("File size should not be larger than 2MB.");
				}
			}

			if (productCreateDTO.CategoryId <= 0)
			{
				errors.Add("Category ID is required.");
			}
			else
			{
				bool isCategoryExist = await _context.Categories.AnyAsync(c => c.Id == productCreateDTO.CategoryId);
				if (!isCategoryExist)
				{
					errors.Add("Category not found.");
				}
			}


			if (errors.Count > 0)
			{
				return errors;
			}

			string fileName = await productCreateDTO.Photo?.CreateFileAsync(_env.WebRootPath, "assets", "img");

			Product product = new Product
			{
				Name = productCreateDTO.Name,
				Description = productCreateDTO.Description,
				CurrentPrice = productCreateDTO.CurrentPrice,
				OldPrice = productCreateDTO.OldPrice,
				CategoryId = (int)productCreateDTO.CategoryId,
				//ProductColors = new List<ProductColor>(),
				//ProductImages = new List<ProductImages>()
			};

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

			return errors;
		}
		public async Task<ProductUpdateDTO> GetProductForUpdateAsync(int id)
		{
			Product product = await _context.Products
				.Include(p => p.Category)
				.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
				.FirstOrDefaultAsync(s => s.Id == id);

			if (product == null) throw new Exception("Product not found");

			//ProductUpdateDTO updateDTO = _mapper.Map<ProductUpdateDTO>(product);


			// ProductUpdateDTO'yu oluşturun ve eski verileri set edin
			ProductUpdateDTO updateDTO = new ProductUpdateDTO
			{
				Name = product.Name,
				Description = product.Description,
				OldPrice = product.OldPrice,
				CurrentPrice = product.CurrentPrice,
				// Diğer özellikleri de set edin...
			};
			updateDTO.CategoryList = GetCategoryList(); // Kategori listesini set et

			return updateDTO;
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
