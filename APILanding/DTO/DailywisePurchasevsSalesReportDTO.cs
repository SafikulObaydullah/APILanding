namespace APILanding.DTO
{
   public class DailywisePurchasevsSalesReportDTO
   {
      public long? ItemId { get; set; }
      public string? strItemName { get; set; }  
      public long? PurchaseQuantity { get; set; }
      public long? PurchasePrice { get; set; }
      public long? SalesQuantity { get; set; }
      public long? SalesPrice { get; set; }
      public long? TotalPurchase { get; set; }
      public long? TotalSales { get; set; }  
      public string? LossStatus { get; set; }   
      public string? ProfitStatus { get; set; } 
   }
}
