using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Domain.Entities
{
	public class Slider:BaseNameableEntity
	{
		public string SubTitle { get; set; } = null!;
		public string Image { get; set; } = null!;
        public int Order { get; set; }
	}
}
