using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Application.Abstarctions.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IColorRepository _colorrepo;
		private readonly IProductRepository _productrepo;
		private readonly ICategoryRepository _categoryrepo;

		public ProductService(AppDbContext context, IMapper mapper, IWebHostEnvironment env, 
			IColorRepository colorrepo,IProductRepository productrepo,ICategoryRepository categoryrepo)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
			_colorrepo = colorrepo;
			_productrepo = productrepo;
			_categoryrepo = categoryrepo;
	
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
		public async Task<ProductCreateDTO> CreatedAsync(ProductCreateDTO dto)
		{
			dto.Colors = await _colorrepo.GetAll().ToListAsync();
			dto.Categories=await _categoryrepo.GetAll().ToListAsync();
			return dto;
		}
		public async Task<bool> Create(ProductCreateDTO productCreateDTO,ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid)
			{
				foreach (var modelError in modelstate.Values.SelectMany(v => v.Errors))
				{
					var errorMessage = modelError.ErrorMessage;
					Console.WriteLine($"ModelState Error: {errorMessage}");
				}
				return false;
			}

			if (await _productrepo.IsExistAsync(p=>p.Name== productCreateDTO.Name))
			{
				modelstate.AddModelError("Name", "Name already exists");
			}

			if (await _productrepo.IsExistAsync(p => p.CurrentPrice<=0))
			{
				modelstate.AddModelError("CurrentPrice", "CurrentPrice must be greater than or equal to 0");
				return false;
			}
			if (await _productrepo.IsExistAsync(p => p.OldPrice <= 0))
			{
				modelstate.AddModelError("OldPrice", "OldPrice must be greater than or equal to 0");
				return false;
			}

			if (productCreateDTO.MainPhoto!=null)
			{
				if (!productCreateDTO.MainPhoto.ValidateType("image/"))
				{
					modelstate.AddModelError("MainPhoto", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!productCreateDTO.MainPhoto.ValidateSize(600))
				{
					modelstate.AddModelError("MainPhoto", "File size should not be larger than 2MB.");
					return false;
				}
			}
			if (productCreateDTO.HoverPhoto != null)
			{
				if (!productCreateDTO.HoverPhoto.ValidateType("image/"))
				{
					modelstate.AddModelError("HoverPhoto", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!productCreateDTO.HoverPhoto.ValidateSize(600))
				{
					modelstate.AddModelError("HoverPhoto", "File size should not be larger than 2MB.");
					return false;
				}
			}

			ProductImages main = new ProductImages()
			{
				IsPrimary = true,
				Url = await productCreateDTO.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img"),
				Alternative = productCreateDTO.Name

			};
			ProductImages hover = new ProductImages()
			{
				IsPrimary = false,
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

			if (productCreateDTO.ColorIds != null)
			{
				foreach (var colorId in productCreateDTO.ColorIds)
				{
					if (!await _colorrepo.IsExistAsync(c => c.Id == colorId))
					{
						modelstate.AddModelError(String.Empty, "This  does not exist");
						return false;
					}
					product.ProductColors.Add(new ProductColor
					{
						ColorId = colorId
					});
				}
			}

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return true;
			
		}
		//public async Task<ProductUpdateDTO> GetProductForUpdateAsync(int id)
		//{
		//	Product product = await _context.Products
		//		.Include(p => p.Category)
		//		.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
		//		.Include(p => p.ProductColors)
		//		.FirstOrDefaultAsync(s => s.Id == id);

		//	if (product == null) throw new Exception("Product not found");

		//	ProductUpdateDTO updateDTO = new ProductUpdateDTO
		//	{
		//		Name = product.Name,
		//		Description = product.Description,
		//		OldPrice = product.OldPrice,
		//		CurrentPrice = product.CurrentPrice,
		//		ProductImages = product.ProductImages,
		//		CategoryId = product.CategoryId,
		//		Categories = await GetCategoriesAsync(),
		//		ColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
		//		Colors = await _context.Colors.ToListAsync(),
		//	};
		//	return updateDTO;
		//}

		//public async Task Update(int id, ProductUpdateDTO updateDTO)
		//{
		//	List<string> errors = new List<string>();

		//	Product product = await _context.Products
		//		.Include(p => p.ProductImages)
		//		.Include(p => p.ProductColors)
		//		.FirstOrDefaultAsync(p => p.Id == id);
		//	updateDTO.Categories = await _context.Categories.ToListAsync();
		//	updateDTO.Colors = await _context.Colors.ToListAsync();
		//	updateDTO.ProductImages = product.ProductImages;

		//	if (product == null) throw new Exception("Product not found");

		//	if (updateDTO.MainPhoto != null)
		//	{
		//		if (!updateDTO.MainPhoto.ValidateType("image/"))
		//		{
		//			errors.Add("File type does not match. Please upload a valid image.");
		//		}

		//		if (!updateDTO.MainPhoto.ValidateSize(600))
		//		{
		//			errors.Add("File size should not be larger than 2MB.");
		//		}
		//	}
		//	if (updateDTO.HoverPhoto != null)
		//	{

		//		if (!updateDTO.HoverPhoto.ValidateType("image/"))
		//		{
		//			errors.Add("File type does not match. Please upload a valid image.");
		//		}

		//		if (!updateDTO.HoverPhoto.ValidateSize(600))
		//		{
		//			errors.Add("File size should not be larger than 2MB.");
		//		}
		//	}
		//	if (updateDTO.HoverPhoto != null)
		//	{

		//		if (!updateDTO.HoverPhoto.ValidateType("image/"))
		//		{
		//			errors.Add("File type does not match. Please upload a valid image.");
		//		}

		//		if (!updateDTO.HoverPhoto.ValidateSize(600))
		//		{
		//			errors.Add("File size should not be larger than 2MB.");
		//		}
		//	}

		//	bool result = await _context.Categories.AnyAsync(c => c.Id == updateDTO.CategoryId);
		//	if (!result) throw new Exception("Category not found");

		//	List<ProductColor> removable = product.ProductColors.Where(pc => !updateDTO.ColorIds.Exists(cId => cId == pc.ColorId)).ToList();
		//	_context.ProductColors.RemoveRange(removable);

		//	List<int> creatable = updateDTO.ColorIds
		//.Where(cId => !product.ProductColors.Any(pc => pc.ColorId == cId))
		//.ToList();
		//	foreach (int cId in creatable)
		//	{
		//		bool ColorResult = await _context.Products.AnyAsync(c => c.Id == cId);
		//		if (!ColorResult) throw new Exception("Color not found");

		//		product.ProductColors.Add(new ProductColor
		//		{
		//			ColorId = cId,
		//		});
		//	}
		//	if (updateDTO.MainPhoto != null)
		//	{
		//		string fileName = await updateDTO.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");

		//		ProductImages existedImg = product.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
		//		existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
		//		product.ProductImages.Remove(existedImg);

		//		product.ProductImages.Add(new ProductImages
		//		{
		//			IsPrimary = true,
		//			Alternative = updateDTO.Name,
		//			Url = fileName,

		//		});
		//	}
		//	if (updateDTO.HoverPhoto != null)
		//	{
		//		string fileName = await updateDTO.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");

		//		ProductImages existedImg = product.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
		//		existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
		//		product.ProductImages.Remove(existedImg);

		//		product.ProductImages.Add(new ProductImages
		//		{
		//			IsPrimary = false,
		//			Alternative = updateDTO.Name,
		//			Url = fileName,
		//		});
		//	}
		//	product.Name = updateDTO.Name;
		//	product.Description = updateDTO.Description;
		//	product.CurrentPrice = updateDTO.CurrentPrice;
		//	product.OldPrice = updateDTO.OldPrice;
		//	product.CategoryId = updateDTO.CategoryId;


		//	await _context.SaveChangesAsync();
		//}

		//public async Task Delete(int id)
		//{
		//	Product existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

		//	if (existed == null)
		//	{
		//		throw new Exception("Product is null");
		//	}

		//	try
		//	{
		//		_context.Products.Remove(existed);
		//		await _context.SaveChangesAsync();
		//	}
		//	catch (Exception)
		//	{
		//		throw new Exception("Product is null");

		//	}

		//	//if (!string.IsNullOrEmpty(existed.Image))
		//	//{
		//	//	existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
		//	//}

		//	await _context.SaveChangesAsync();
		//}

		//public Task SoftDeleteAsync(int id)
		//{
		//	throw new NotImplementedException();
		//}


	}
}
