namespace APILanding.DTO
{
   public class LandingReportDTO
   {
      public string Month { get; set; }
      public long Year { get; set;}
      public decimal? TotalPurchaseAmount { get; set;}
      public decimal? TotalSalesAmount { get; set; }
      public string? Status { get; set; }
     
   }
}
