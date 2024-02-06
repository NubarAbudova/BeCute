using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs
{
	public class ItemVM<T> where T : class
	{
        public List<T> Items { get; set; }
    }
}
