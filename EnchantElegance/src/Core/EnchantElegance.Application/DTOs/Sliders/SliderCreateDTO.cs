using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Application.DTOs.Sliders
{
	public class SliderCreateDTO
	{
        public string Name { get; set; }
        public string SubTitle   { get; set; }
        public string Description { get; set; }
		public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public int Order { get; set; }

	};
}
