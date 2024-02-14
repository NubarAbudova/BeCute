using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Application.Abstarctions.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using EnchantElegance.Application.ViewModels;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ProductService : IProductService
	{
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IColorRepository _colorrepo;
		private readonly IProductRepository _productrepo;
		private readonly ICategoryRepository _categoryrepo;

		public ProductService( IMapper mapper, IWebHostEnvironment env, 
			IColorRepository colorrepo,IProductRepository productrepo,ICategoryRepository categoryrepo)
		{
			_mapper = mapper;
			_env = env;
			_colorrepo = colorrepo;
			_productrepo = productrepo;
			_categoryrepo = categoryrepo;
		}
		public async Task<PaginationVM<Product>> GetAllAsync(int page=1, int take=5)
		{
			ICollection<Product> products = await _productrepo.GetPagination(skip: (page - 1) * take, take: take, includes: new string[] { nameof(Category), nameof(ProductImages), "ProductColors", "ProductColors.Color" })
				.Where(p => p.ProductImages.Any(pi => pi.IsPrimary == true)).ToListAsync();

			int count = await _productrepo.GetAll().CountAsync();

			double totalpage = Math.Ceiling((double)count / take);
			PaginationVM<Product> productVM = new PaginationVM<Product>
			{
				Items = products,
				CurrentPage = page,
				TotalPage = totalpage
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
			if (!modelstate.IsValid) return false;

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
			await _productrepo.AddAsync(product);
			await _productrepo.SaveChangesAsync();
			return true;
			
		}

		public async Task<ProductUpdateDTO> GetProductForUpdateAsync(int id,ProductUpdateDTO updateDTO)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Product exist = await _productrepo.GetByIdAsync(id,includes:new string[] { nameof(Category), nameof(ProductImages), "ProductColors", "ProductColors.Color" });

			if (exist == null) throw new Exception("Not Found");

			if(exist.ProductImages!=null)
			{
				updateDTO.ProductImages = exist.ProductImages.ToList();

			}
			updateDTO.Name = exist.Name.Trim();
			updateDTO.CurrentPrice = exist.CurrentPrice;
			updateDTO.OldPrice = exist.OldPrice;
			updateDTO.Description=exist.Description;
			updateDTO.Categories = await _categoryrepo.GetAll().ToListAsync();
			updateDTO.Colors= await _colorrepo.GetAll().ToListAsync();
			if (exist.ProductColors != null)
			{
				updateDTO.ColorIds = exist.ProductColors.Select(c => c.ColorId).ToList();

			}

			return updateDTO;
		}

		public async Task<bool> Update(int id, ProductUpdateDTO updateDTO,ModelStateDictionary modelstate)
		{
			if (id <= 0) throw new Exception("Bad Request");

			if (!modelstate.IsValid) return false;
			Product existed = await _productrepo.GetByIdAsync(id);
			List<Product> products = await _productrepo.GetAll(false, false, includes: new string[] { "ProductColors", "ProductImages" }).ToListAsync();

			if (existed is null) throw new Exception("Not Found");

			if (await _productrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _categoryrepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Product is already exist");
				return false;
			}

			if (updateDTO.MainPhoto != null)
			{
				if (!updateDTO.MainPhoto.ValidateType("image/"))
				{
					modelstate.AddModelError("MainPhoto", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!updateDTO.MainPhoto.ValidateSize(600))
				{
					modelstate.AddModelError("MainPhoto", "File size should not be larger than 2MB.");
					return false;
				}
			}
			if (updateDTO.HoverPhoto != null)
			{
				if (!updateDTO.HoverPhoto.ValidateType("image/"))
				{
					modelstate.AddModelError("HoverPhoto", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!updateDTO.HoverPhoto.ValidateSize(600))
				{
					modelstate.AddModelError("HoverPhoto", "File size should not be larger than 2MB.");
					return false;
				}
			}
			if (updateDTO.MainPhoto is not null)
			{
				string fileName = await updateDTO.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");

				ProductImages existedImg = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
				existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
				existed.ProductImages.Remove(existedImg);

				existed.ProductImages.Add(new ProductImages
				{
					IsPrimary = true,
					Alternative = updateDTO.Name,
					Url = fileName
				});

			}
			if (updateDTO.HoverPhoto is not null)
			{
				string fileName = await updateDTO.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "imgs");

				ProductImages existedImg = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
				existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
				existed.ProductImages.Remove(existedImg);

				existed.ProductImages.Add(new ProductImages
				{
					IsPrimary = false,
					Alternative = updateDTO.Name,
					Url = fileName
				});

			}

			if (updateDTO.ImageIds is null)
			{
				updateDTO.ImageIds = new List<int>();
			}
			if (existed.ProductImages != null)
			{
				List<ProductImages> removeable = existed.ProductImages.Where(pi => !updateDTO.ImageIds.Exists(imgId => imgId == pi.Id) && pi.IsPrimary == null).ToList();
				foreach (ProductImages removedImg in removeable)
				{
					removedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
					existed.ProductImages.Remove(removedImg);
				}
			}
			else
			{
				existed.ProductImages=new List<ProductImages>();
			}
			


			foreach (IFormFile photo in updateDTO.Photos ?? new List<IFormFile>())
			{
				if (!photo.ValidateType("image/"))
				{
					continue;
				}
				if (!photo.ValidateSize(600))
				{
					continue;
				}

				existed.ProductImages.Add(new ProductImages
				{
					IsPrimary = null,
					Alternative = updateDTO.Name,
					Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images")
				});
			}

			existed.Name = updateDTO.Name;
			existed.Description = updateDTO.Description;
			existed.OldPrice =updateDTO.OldPrice;
			existed.CurrentPrice = updateDTO.CurrentPrice;


			_productrepo.Update(existed);
			await _productrepo.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Product exist = await _productrepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			List<Product> products = await _productrepo.GetAll(false, false, includes: new string[] {"ProductImages" }).ToListAsync();
		
			foreach (ProductImages image in exist.ProductImages ?? new List<ProductImages>())
			{
				image.Url.DeleteFile(_env.WebRootPath, "assets", "img");
			}
			_productrepo.Delete(exist);
			await _productrepo.SaveChangesAsync();
			return true;
		}

		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		
	}
}
