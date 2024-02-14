using EnchantElegance.Domain.Entities;

namespace EnchantElegance.ViewModels
{
    public class DetailVM
    {
        public Product Product { get; set; }

        public List<Product> RelatedProducts { get; set; }
    }
}
