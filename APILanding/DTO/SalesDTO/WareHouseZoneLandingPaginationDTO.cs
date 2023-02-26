namespace APILanding.DTO.SalesDTO
{
   public class WareHouseZoneLandingPaginationDTO
   {
      public List<WareHouseZoneDTO> Data { get; set; }
      public long currentPage { get; set; }
      public long totalCount { get; set; }
      public long pageSize { get; set; }
   }
}
