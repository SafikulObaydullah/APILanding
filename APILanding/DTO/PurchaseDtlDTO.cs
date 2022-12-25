namespace APILanding.DTO
{
   public class PurchaseDtlDTO
   {
      public long IntDetailsId { get; set; }

      public long? IntPurchaseId { get; set; }

      public long IntItemId { get; set; }

      public long NumItemQuantity { get; set; }

      public long? NumUnitPrice { get; set; }

      public bool? IsActive { get; set; }
   }
}
