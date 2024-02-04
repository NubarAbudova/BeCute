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
		//public async Task<IActionResult> Update(int id)
		//{
		//	if (id <= 0) BadRequest();
		//	Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
		//	if (existed == null) return NotFound();

		//	UpdateSliderVM updateSliderVM = new UpdateSliderVM
		//	{
		//		Description = existed.Description,
		//		Title = existed.Title,
		//		SubTitle = existed.SubTitle,
		//		Order = existed.Order,
		//		Image = existed.Image
		//	};
		//	await _context.SaveChangesAsync();
		//	return View(updateSliderVM);
		//}
		//[HttpPost]
		//public async Task<IActionResult> Update(int id, UpdateSliderVM updateSliderVM)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return View(updateSliderVM);
		//	}
		//	Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

		//	if (existed == null) return NotFound();

		//	if (updateSliderVM.Photo != null)
		//	{
		//		if (!updateSliderVM.Photo.ValidateType("image/"))
		//		{
		//			ModelState.AddModelError("Photo", "File type is not compatible");
		//			return View(existed);
		//		}
		//		if (updateSliderVM.Photo.ValidateSize(2 * 1024))
		//		{
		//			ModelState.AddModelError("Photo", "File size should not be larger than 2MB");
		//			return View(existed);
		//		}
		//	}

		//	string fileName = await updateSliderVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "slider");
		//	existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "slider");
		//	existed.Image = fileName;



		//	existed.Title = updateSliderVM.Title;
		//	existed.Description = updateSliderVM.Description;
		//	existed.SubTitle = updateSliderVM.SubTitle;
		//	existed.Order = updateSliderVM.Order;
		//	await _context.SaveChangesAsync();
		//	return RedirectToAction(nameof(Index));
		//}
		//public async Task<IActionResult> Delete(int id)
		//{
		//	if (id <= 0) return BadRequest();

		//	Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
		//	if (existed == null) return NotFound();

		//	existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "slider");

		//	//string path = Path.Combine(_env.WebRootPath, "assets/images/slider", existed.Image);
		//	//if (System.IO.File.Exists(path))
		//	//{
		//	//    System.IO.File.Delete(path);
		//	//}
		//	_context.Sliders.Remove(existed);
		//	await _context.SaveChangesAsync();
		//	return RedirectToAction(nameof(Index));

		//}
	}
}