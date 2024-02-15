using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantElegance.Application.ServiceRegistration
{
	public static class ServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddFluentValidationAutoValidation()
				.AddFluentValidationClientsideAdapters()
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
