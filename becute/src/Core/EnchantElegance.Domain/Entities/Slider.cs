namespace EnchantElegance.Domain.Entities
{
	public class Slider:BaseNameableEntity
	{
		public string SubTitle { get; set; } = null!;
		public string Image { get; set; } = null!;
        public int Order { get; set; }
	}
}
