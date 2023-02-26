using APILanding.DTO;
using APILanding.DTO.SalesDTO;
using APILanding.Helper;
using APILanding.Models.Data;
using APILanding.Models.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APILanding.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class SalesController : ControllerBase
   {
      private readonly ModelDbContext _context;
      
      public SalesController(ModelDbContext context)
      {
         this._context = context;
      }
      #region
      [HttpPost]
      [Route("CreateWareHouseZone")]
      public async Task<MessageHelper> CreateWareHouseZone(WareHouseZoneDTO obj)
      {
         var existData = await _context.TblWareHouseZones.Where(x => x.IntShipPointId == obj.IntShipPointId && x.IsActive == true).FirstOrDefaultAsync();
         if (existData != null)
         {
            throw new Exception($"{obj.IntShipPointId} Already Exists");
         }
         TblWareHouseZone model = new TblWareHouseZone()
         {
            IntBusinessUnitId = obj.IntBusinessUnitId,
            IntShipPointId = obj.IntShipPointId,
            IsActive = true,
            IntAccountId = obj.IntAccountId,
            IntRouteId = obj.IntRouteId,
            IntTransportZoneId = obj.IntTransportZoneId,
            IntWhid = obj.IntWhid,
         };
         await _context.TblWareHouseZones.AddAsync(model);
         await _context.SaveChangesAsync();
         return new MessageHelper()
         {
            Message = "Successfully Saved",
            StatusCode = 200,
         };
      }
      [HttpGet]
      [Route("GetWareHouseZone")]
      public async Task<WareHouseZoneLandingPaginationDTO> GetWareHouseZone(long BusinessUnitId,long ShipPointId,long PageNo,long PageSize,string viewOrder)
      {
         IQueryable<WareHouseZoneDTO> data =  (from a in _context.TblWareHouseZones
                                             where a.IntBusinessUnitId == BusinessUnitId
                                             && a.IntShipPointId == ShipPointId
                                             && a.IsActive == true
                                             select new WareHouseZoneDTO()
                                             {
                                                IntAccountId = a.IntAccountId,
                                                IntBusinessUnitId = a.IntBusinessUnitId,
                                                IntShipPointId = a.IntShipPointId,
                                                IntTransportZoneId = a.IntTransportZoneId,
                                                IntWhid = a.IntWhid,
                                                IntAutoId = a.IntAutoId,
                                                IntRouteId = a.IntRouteId,
                                             });
         if(data == null)
         {
            throw new Exception("Data Not Found");
         }
         if (PageNo <= 0)
            PageNo = 1;
         else
         {
            if (viewOrder.ToUpper() == "ASC")
               data = data.OrderBy(o => o.IntAutoId);
            else if (viewOrder.ToUpper() == "DESC")
               data = data.OrderByDescending(o => o.IntAutoId);
         }
         if(PageNo<= 0)
            PageNo = 1;
         var getData = PagingList<WareHouseZoneDTO>.CreateAsync(data, PageNo, PageSize);
         long index = 1;
         foreach(var oData in getData)
            oData.SL = index++;
         WareHouseZoneLandingPaginationDTO objInfo = new WareHouseZoneLandingPaginationDTO();
         objInfo.Data = getData;
         objInfo.currentPage = PageNo;
         objInfo.totalCount = data.Count();
         objInfo.pageSize = PageSize;
         return objInfo;
      }
      [HttpPut]
      [Route("EditWareHouseZone")]
      public async Task<MessageHelper> EditWareHouseZone(WareHouseZoneDTO obj)
      {
         var updateData = await _context.TblWareHouseZones.Where(x=>x.IsActive == true && x.IntAutoId == obj.IntAutoId).FirstOrDefaultAsync();
         if (updateData == null)
         {
            throw new Exception("Data Not Found");
         }
         updateData.IntBusinessUnitId = obj.IntBusinessUnitId;
         updateData.IntShipPointId = obj.IntShipPointId;
         updateData.IsActive = true;
         updateData.IntAccountId = obj.IntAccountId;
         updateData.IntRouteId = obj.IntRouteId;
         updateData.IntTransportZoneId = obj.IntTransportZoneId;
         updateData.IntWhid = obj.IntWhid;
         
          _context.TblWareHouseZones.Update(updateData);
         await _context.SaveChangesAsync();
         return new MessageHelper()
         {
            Message = "Successfully Updated",
            StatusCode = 200,
         };
      }
      [HttpGet]
      [Route("DeleteWareHouseZone")]
      public async Task<MessageHelper> DeleteWareHouseZone(long AutoId)
      {
         var deletedata = await _context.TblWareHouseZones.Where(x=>x.IntAutoId == AutoId && x.IsActive == true).FirstOrDefaultAsync();
         if (deletedata == null)
         {
            throw new Exception("Data Not Found");
         }
         deletedata.IsActive = false;  
         _context.TblWareHouseZones.Update(deletedata); 
         await _context.SaveChangesAsync();
         return new MessageHelper()
         {
            Message = "Deleted Successfully",
            StatusCode = 200,
         };
      }
      #endregion

   }
}
