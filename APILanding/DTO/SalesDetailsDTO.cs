namespace APILanding.DTO
{
   public class SalesDetailsDTO
   {
      public long? IntCustomerId { get; set; }
      public DateTime? DteSalesDate { get; set; }
      public long? IntSalesId { get; set; }
      public long? IntItemId { get; set; } 
      public long? NumItemQuantity { get; set; }
      public long? NumUnitPrice { get; set; }
      public bool? IsActive { get; set; }
   }
}
