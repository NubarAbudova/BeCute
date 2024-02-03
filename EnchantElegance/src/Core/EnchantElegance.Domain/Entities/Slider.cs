using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
	public class Slider:BaseNameableEntity
	{
		public string SubTitle { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public int Order { get; set; }
	}
}
