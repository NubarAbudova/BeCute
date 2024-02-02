﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EnchantElegance.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; }=null!;
        public bool IsActive { get; set; }
    }
}