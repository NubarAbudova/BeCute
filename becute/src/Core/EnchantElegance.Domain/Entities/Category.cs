namespace EnchantElegance.Domain.Entities
{
    public class Category:BaseNameableEntity
    {
        public string Image { get; set; }
        //Relational Properties
        public ICollection<Product>? Products { get; set; }
		public ICollection<ProductImages>? ProductImages { get; set; }

	}
}
    