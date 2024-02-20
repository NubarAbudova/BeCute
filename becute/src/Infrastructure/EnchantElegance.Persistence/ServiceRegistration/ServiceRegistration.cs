using System.Reflection;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.Persistence.Implementations.Repositories;
using EnchantElegance.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantElegance.Persistence.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));


			services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

			services.AddScoped<IMailService, MailService>();
            services.AddScoped<LayoutService>();


            //Identity
            services.AddAuthentication();
			services.AddScoped<IAccountService, AccountService>();

            //Slider
            services.AddScoped<ISliderService, SliderService>();
			services.AddScoped<ISliderRepository, SliderRepository>();

			//Category
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();

            //Product
			services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository,ProductRepository>();

            //Color
			services.AddScoped<IColorService,ColorService >();
			services.AddScoped<IColorRepository, ColorRepository>();

            //Basket
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            //BasketItem

            services.AddScoped<IBasketItemRepository, BasketItemRepository>();

            //Client
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientRepository, ClientRepository>();

			//Employee
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			return services;
        }
    }
}
