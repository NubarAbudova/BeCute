using EnchantElegance.Application.DTOs.Sliders;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class SliderController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;
		public SliderController(AppDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			return View(sliders);
		}
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(SliderCreateDTO sliderDTO)
		{
			if(ModelState.IsValid) return View(sliderDTO);

			if (!sliderDTO.Photo.ValidateType("image/"))
			{
				ModelState.AddModelError("Photo", "File type is not compatible");
				return View();
			}
			if (!sliderDTO.Photo.ValidateSize(2 * 1024))
			{
				ModelState.AddModelError("Photo", "File size should not be larger than 2MB");
				return View();
			}

			string fileName = await sliderDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");

			Slider slider = new Slider
			{
				Image = fileName,
				Name = sliderDTO.Name,
				SubTitle = sliderDTO.SubTitle, 
				Description = sliderDTO.Description,
				Order = sliderDTO.Order,
			};

			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	
		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0) BadRequest();

			Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (slider == null) return NotFound();

			SliderUpdateDTO updateDTO = new SliderUpdateDTO
			{
				Name= slider.Name,
				Description = slider.Description,
				SubTitle = slider.SubTitle,
				Order = slider.Order,
				Image = slider.Image
			};
			await _context.SaveChangesAsync();
			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, SliderUpdateDTO updateDTO)
		{
			if (!ModelState.IsValid) return View(updateDTO);
	
			Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (existed == null) return NotFound();

			if (updateDTO.Photo != null)
			{
				if (!updateDTO.Photo.ValidateType("image/"))
				{
					ModelState.AddModelError("Photo", "File type is not compatible");
					return View(existed);
				}
				if (updateDTO.Photo.ValidateSize(2 * 1024))
				{
					ModelState.AddModelError("Photo", "File size should not be larger than 2MB");
					return View(existed);
				}
			}


			string fileName = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");
			if (!string.IsNullOrEmpty(existed.Image))
			{
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "slider");
			}

			existed.Image = fileName;

			existed.Name = updateDTO.Name;
			existed.Description = updateDTO.Description;
			existed.SubTitle = updateDTO.SubTitle;
			existed.Order = updateDTO.Order;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0) return BadRequest();

			Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (existed == null) return Json(new { status = 404 });

			try
			{
				_context.Sliders.Remove(existed);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{

				return Json(new { status = 500 });
			}

			if (!string.IsNullOrEmpty(existed.Image))
			{
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "slider");
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		//public async Task<IActionResult> Details(int id)
		//{
		//	if (id <= 0) BadRequest();
		//	Slider slider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);

		//	List<Slider> sliders = await _context.Sliders
		//		.Where(s => s.Id == id)
		//		.Include(s => s.Title)
		//		.Include(s => s.SubTitle)
		//		.Include(s => s.Description)
		//		.Include(s => s.Image)
		//		.ToListAsync();

		//	if (slider == null) return NotFound();
		//	return View(slider);
		//}
	}
}
