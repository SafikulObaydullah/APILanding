namespace APILanding.DTO
{
   public class PurchaseSalseDetailsDTO
	{
		public long? IntSupplierId { get; set; }

		public DateTime? DtePurchaseDate { get; set; }
		public long? IntPurchaseId { get; set; }

		public long? IntItemId { get; set; }

		public long? PurchaseNumItemQuantity { get; set; }

		public long? PurchaseNumUnitPrice { get; set; }
		public long? SalseNumItemQuantity { get; set; }

		public long? SalesNumUnitPrice { get; set; }
		public string? strItemName { get; set; }
		public bool? IsActive { get; set; }

	}
}
