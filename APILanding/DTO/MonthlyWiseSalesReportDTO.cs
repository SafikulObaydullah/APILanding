namespace APILanding.DTO
{
   public class MonthlyWiseSalesReportDTO
   {
      public long? itemId { get; set; }
      public string? strItemName { get; set; }
      public long? Quantity { get; set; }
      public long? UnitPrice { get; set; }
      public long? TotalPurchase { get; set; }
   }
}
