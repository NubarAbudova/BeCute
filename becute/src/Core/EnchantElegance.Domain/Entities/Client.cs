using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
    public class Client:BaseNameableEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Profession { get; set; }
        public string Description { get; set; }
    }
}
