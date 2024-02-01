﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
    public class Category:BaseNameableEntity
    {
        public string Description { get; set; } = null!;

        //Relational Properties
        public ICollection<Product>? Products { get; set; } 
    }
}
    