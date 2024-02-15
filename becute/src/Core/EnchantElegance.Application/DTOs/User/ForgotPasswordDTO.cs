using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs
{
	public class ForgotPasswordDTO
	{
		[Required, MaxLength(256), DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
    }
}
