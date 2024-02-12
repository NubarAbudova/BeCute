namespace EnchantElegance.Domain.Entities
{
	public class ProductImages:BaseEntity
	{
        public string Url { get; set; }
        public bool? IsPrimary { get; set; }
        public string  Alternative { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
