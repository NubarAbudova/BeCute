using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Controllers
{
	public class AboutUs : Controller
	{
        private readonly IClientRepository _clientrepo;
        private readonly IEmployeeRepository _employeerepo;

        public AboutUs(IClientRepository clientrepo,IEmployeeRepository employeerepo)
		{
            _clientrepo = clientrepo;
            _employeerepo = employeerepo;
        }
		public async Task<IActionResult> Index()
		{
			List<Client> clients = await _clientrepo.GetAll().ToListAsync();
			List<Employee> employees = await _employeerepo.GetAll().ToListAsync();

			AboutUsVM aboutUs = new AboutUsVM()
			{
				Clients = clients,
				Employees = employees
			};

			return View(aboutUs);
		}
	}
}
