﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnchantElegance.Application.DTOs
{
	public class ProductUpdateDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
		public int CategoryId { get; set; }
		public int ColorId { get; set; }
		public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

	}
}
