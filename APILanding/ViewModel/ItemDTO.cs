namespace APILanding.ViewModel
{
	public class CommonITemDTO
	{
		public List<ItemDTO> itemList { get; set; }
	}
	public class ItemDTO
	{
		public long IntItemId { get; set; }

		public string? StrItemName { get; set; }

		public long? NumStockQuantity { get; set; }

		public bool? IsActive { get; set; }
	}
}
