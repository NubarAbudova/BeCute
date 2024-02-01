using System.Diagnostics;
using EnchantElegance.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
    public class CategoriesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


    }
}