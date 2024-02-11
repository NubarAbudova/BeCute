using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class ColorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
