namespace APILanding.DTO
{
   public class SalesDetailsCommonDTO
   {
      public long? IntCustomerId { get; set; }
      public DateTime? DteSalesDate { get; set; }
      public long? IntSalesId { get; set; }
      public bool IsActive { get; set; }      
      public List<SalesDetailsRowDTO> SalesDetails { get; set; }  
      
   }
}
