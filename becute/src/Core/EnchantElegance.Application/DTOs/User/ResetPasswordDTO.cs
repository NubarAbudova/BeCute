using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs
{
	public class ResetPasswordDTO
	{
		[DataType(DataType.Password)]
        public string NewPassword { get; set; }
		[DataType(DataType.Password),Compare(nameof(NewPassword))]
		public string ConfirmPassword { get; set; }
	}
}
