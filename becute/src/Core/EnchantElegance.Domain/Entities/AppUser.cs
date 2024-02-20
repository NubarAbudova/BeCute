using Microsoft.AspNetCore.Identity;

namespace EnchantElegance.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; }=null!;
        public bool IsActive { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public List<BasketItem> BasketItems { get; set; }

    }
}
