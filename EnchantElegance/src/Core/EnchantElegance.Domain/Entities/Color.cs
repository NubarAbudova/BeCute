namespace EnchantElegance.Domain.Entities
{
	public class Color:BaseNameableEntity
	{
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
