using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IHttpContextAccessor _http;

		public ProductService(AppDbContext context, IMapper mapper, IWebHostEnvironment env, IHttpContextAccessor http)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
			_http = http;
		}

		public async Task<ItemVM<Product>> GetAllAsync(int page, int take)
		{
			List<Product> products = await _context.Products
				.Include(p => p.Category)
				.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
				.ToListAsync();

			ItemVM<Product> productVM = new ItemVM<Product>
			{
				Items = products,
			};

			return productVM;
		}
		public async Task<ProductCreateDTO> GetProductCreateDTO()
		{
			var productCreateDTO = new ProductCreateDTO();

			productCreateDTO.Categories = await GetCategoriesAsync();

			List<Color> colors = await _context.Colors.ToListAsync();

			productCreateDTO.ColorIds = colors.Select(c => c.Id).ToList();

			return productCreateDTO;
		}

		public async Task<List<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.ToListAsync();
		}
		public async Task<List<string>> Create(ProductCreateDTO productCreateDTO)
		{
			productCreateDTO.Categories = await _context.Categories.ToListAsync();
			productCreateDTO.Colors = await _context.Colors.ToListAsync();
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
			if (errors.Count > 0)
			{
				_http.HttpContext.Response.Redirect("/your-error-path");
				return errors;
			}
			if (productCreateDTO.MainPhoto != null)
			{

				if (!productCreateDTO.MainPhoto.ValidateType("image/"))
				{
					errors.Add("File type does not match. Please upload a valid image.");
				}

				if (!productCreateDTO.MainPhoto.ValidateSize(600))
				{
					errors.Add("File size should not be larger than 2MB.");
				}
			}
			if (productCreateDTO.HoverPhoto != null)
			{

				if (!productCreateDTO.HoverPhoto.ValidateType("image/"))
				{
					errors.Add("File type does not match. Please upload a valid image.");
				}

				if (!productCreateDTO.HoverPhoto.ValidateSize(600))
				{
					errors.Add("File size should not be larger than 2MB.");
				}
			}
			if (productCreateDTO.HoverPhoto != null)
			{

				if (!productCreateDTO.HoverPhoto.ValidateType("image/"))
				{
					errors.Add("File type does not match. Please upload a valid image.");
				}

				if (!productCreateDTO.HoverPhoto.ValidateSize(600))
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

			foreach (int ColorId in productCreateDTO.ColorIds)
			{
				bool ColorResult = await _context.Colors.AnyAsync(c => c.Id == ColorId);
				if (!ColorResult) throw new Exception("This color does not found");

			}


			if (errors.Count > 0)
			{
				_http.HttpContext.Response.Redirect("/your-error-path");
				_http.HttpContext.Items["Message"] = string.Join(", ", errors);
				return errors;
			}

			ProductImages main = new ProductImages()
			{
				IsPrimary = true,
				Url = await productCreateDTO.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img"),
				Alternative = productCreateDTO.Name

			};
			ProductImages hover = new ProductImages()
			{
				IsPrimary = true,
				Url = await productCreateDTO.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img"),
				Alternative = productCreateDTO.Name

			};
			Product product = new Product
			{
				Name = productCreateDTO.Name,
				Description = productCreateDTO.Description,
				CurrentPrice = productCreateDTO.CurrentPrice,
				OldPrice = productCreateDTO.OldPrice,
				CategoryId = (int)productCreateDTO.CategoryId,
				ProductColors = new List<ProductColor>(),
				ProductImages = new List<ProductImages> { main, hover }
			};
			if (errors.Count > 0)
			{
				_http.HttpContext.Response.Redirect("/your-error-path");
				_http.HttpContext.Items["Message"] = string.Join(", ", errors);
				return errors;
			}

			foreach (IFormFile photo in productCreateDTO.Photos)
			{
				if (!photo.ValidateType("image/"))
				{

					continue;
				}
				if (!photo.ValidateSize(600))
				{

					continue;
				}

				product.ProductImages.Add(new ProductImages
				{
					IsPrimary = true,
					Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
					Alternative = productCreateDTO.Name
				});
			}
			foreach (int colorId in productCreateDTO.ColorIds)
			{
				ProductColor productColor = new ProductColor
				{
					ColorId = colorId,
				};
				product.ProductColors.Add(productColor);
			}

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

			return errors;
		}
		public async Task<ProductUpdateDTO> GetProductForUpdateAsync(int id)
		{
			Product product = await _context.Products
				.Include(p => p.Category)
				.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
				.Include(p => p.ProductColors)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (product == null) throw new Exception("Product not found");

			ProductUpdateDTO updateDTO = new ProductUpdateDTO
			{
				Name = product.Name,
				Description = product.Description,
				OldPrice = product.OldPrice,
				CurrentPrice = product.CurrentPrice,
				CategoryId = product.CategoryId,
				Categories = await GetCategoriesAsync(),
				ColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
				Colors = await _context.Colors.ToListAsync(),

			};

			return updateDTO;
		}


		public async Task Update(int id, ProductUpdateDTO updateDTO)
		{

			updateDTO.Categories = await _context.Categories.ToListAsync();
			updateDTO.Colors = await _context.Colors.ToListAsync();

			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null) throw new Exception("Product not found");

			bool result = await _context.Categories.AnyAsync(c => c.Id == updateDTO.CategoryId);
			if (!result) throw new Exception("Category not found");

			List<ProductColor> removable = product.ProductColors.Where(pc => !updateDTO.ColorIds.Exists(cId => cId == pc.ColorId)).ToList();
			_context.ProductColors.RemoveRange(removable);

			List<int> creatable = updateDTO.ColorIds
		.Where(cId => !product.ProductColors.Any(pc => pc.ColorId == cId))
		.ToList();
			foreach (int cId in creatable)
			{
				bool ColorResult = await _context.Products.AnyAsync(c => c.Id == cId);
				if (!ColorResult) throw new Exception("Color not found");

				product.ProductColors.Add(new ProductColor
				{
					ColorId = cId,
				});
			}

			product.Name = updateDTO.Name;
			product.Description = updateDTO.Description;
			product.CurrentPrice = updateDTO.CurrentPrice;
			product.OldPrice = updateDTO.OldPrice;
			product.CategoryId = updateDTO.CategoryId;

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
