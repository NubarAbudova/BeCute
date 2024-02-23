using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Controllers
{
	public class HomeController : Controller
	{
        private readonly ISliderRepository _sliderrepo;
        private readonly ICategoryRepository _categoryrepo;
        private readonly IColorRepository _colorrepo;
        private readonly IProductRepository _productrepo;
        private readonly IClientRepository _clientrepo;

        public HomeController(ISliderRepository sliderrepo,ICategoryRepository categoryrepo,IColorRepository colorrepo,
			IProductRepository productrepo,IClientRepository clientrepo)
		{
            _sliderrepo = sliderrepo;
            _categoryrepo = categoryrepo;
            _colorrepo = colorrepo;
            _productrepo = productrepo;
            _clientrepo = clientrepo;
        }
		public async Task<IActionResult> Index()
		{
			List<Slider> sliders = await _sliderrepo.GetAll().OrderBy(s=>s.Order).ToListAsync();
			List<Category> categories = await _categoryrepo.GetAll().ToListAsync();
			List<Color> colors = await _colorrepo.GetAll().ToListAsync();
			List<Product> products = await _productrepo.GetAll().Include(p=>p.ProductImages).ToListAsync();
			List<Client> clients = await _clientrepo.GetAll().ToListAsync();

			HomeVM homeVM = new HomeVM()
			{
				Sliders = sliders,
				Categories = categories,
				Products = products,
				Colors=colors,
				Clients = clients
			};

			return View(homeVM);
		}
		public IActionResult PrivacyandPolicy()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult FAQ()
		{
			return View();
		}

	}
}
