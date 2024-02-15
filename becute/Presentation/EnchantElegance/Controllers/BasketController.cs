using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
	public class BasketController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
