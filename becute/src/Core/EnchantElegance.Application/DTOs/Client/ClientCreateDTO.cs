using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Application.DTOs
{
    public class ClientCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Profession { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
