namespace EnchantElegance.Domain.Entities
{
	public class Color:BaseNameableEntity
	{
        public string No { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
