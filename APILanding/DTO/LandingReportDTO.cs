namespace APILanding.DTO
{
   public class LandingReportDTO
   {
      public string? Monthname { get; set; }
      public string? Year { get; set;}
      public long? TotalPurchaseAmount { get; set;}
      public long? TotalSalesAmount { get; set; }
      public string ProfitSatus { get; set; }
      public string LossStatus { get; set; }
   }
}
