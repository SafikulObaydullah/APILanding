namespace APILanding.DTO
{
   public class SalesDetailsRowDTO
   {
      public long IntItemId { get; set; }
      public string? strItemName { get; set; }
      public long? NumItemQuantity { get; set; }
      public long? NumUnitPrice { get; set; }
      public bool? IsActive { get; set; }
   }
}
