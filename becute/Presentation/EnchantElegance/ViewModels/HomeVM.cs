using EnchantElegance.Domain.Entities;

namespace EnchantElegance.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders {  get; set; }
        public List<Category> Categories { get; set; }
		public List<Product> Products { get; set; }
		public List<Color> Colors { get; set; }
		public List<Client> Clients { get; set; }

	}
}
