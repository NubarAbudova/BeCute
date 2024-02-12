namespace EnchantElegance.Application.DTOs
{
	public class ItemVM<T> where T : class
	{
        public List<T> Items { get; set; }
    }
}
