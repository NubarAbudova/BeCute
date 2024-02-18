using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Controllers
{
	public class AboutUs : Controller
	{
		private readonly AppDbContext _context;

		public AboutUs(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			List<Client> clients = await _context.Clients.ToListAsync();
			List<Employee> employees = await _context.Employees.ToListAsync();

			AboutUsVM aboutUs = new AboutUsVM()
			{
				Clients = clients,
				Employees = employees
			};

			return View(aboutUs);
		}
	}
}
