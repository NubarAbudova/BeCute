using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using EnchantElegance.Application.DTOs.User;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IMailService
	{

		Task SendEmailAsync(MailRequestDTO mailRequest);
	}
}
