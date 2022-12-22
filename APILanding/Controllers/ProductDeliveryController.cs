using APILanding.Models.Data;
using APILanding.Models.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace APILanding.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ProductDeliveryController : ControllerBase
   {
      private readonly ModelDbContext _context;
      public ProductDeliveryController(ModelDbContext context)
      {
         this._context = context;
      }
      [HttpPost]
      [Route("CreatePartnerType")]
      public IActionResult CreatePartnerType(TblPartnerType tblPartnerType )
      {
         try
         {
            TblPartnerType type = new TblPartnerType()
            {
               StrPartnerTypeName = tblPartnerType.StrPartnerTypeName,
               IsActive = tblPartnerType.IsActive
            };
            _context.TblPartnerTypes.Add(tblPartnerType);
            _context.SaveChanges();
         }
         catch(Exception ex)
         {
            throw;
         }
         return Ok();
      }
      [HttpPost]
      [Route("CreateListofItem")]
      public IActionResult CreateListofItem(List<TblItem> obj)
      {
         try
         {
            var existdata = _context.TblItems.Where(x => x.StrItemName.ToLower() == obj.Select(a => a.StrItemName.ToLower()).FirstOrDefault()).Count();
            if (existdata > 0) throw new Exception("Items already Exists");
            List<TblItem> itemlist = new List<TblItem>();
            List<TblItem> updatelist = new List<TblItem>();
            if(obj.Count>0)
            {
               foreach(var item in obj)
               {
                  if(item.IntItemId==0)
                  {
                     TblItem tblItem = new TblItem()
                     {
                        StrItemName = item.StrItemName,
                        NumStockQuantity = item.NumStockQuantity,
                        IsActive = item.IsActive
                     };
                     itemlist.Add(tblItem);
                     
                  }
                  else
                  {
                     TblItem tbl = _context.TblItems.Where(x => x.IntItemId == item.IntItemId).FirstOrDefault();
                     tbl.StrItemName = item.StrItemName;
                     tbl.NumStockQuantity = item.NumStockQuantity;
                     tbl.IsActive = item.IsActive;

                     updatelist.Add(tbl);
                  }  
               }
               if(itemlist.Count>0)
               {
                  _context.TblItems.AddRange(itemlist);
                  _context.SaveChanges();
               }
               else if(updatelist.Count()>0)
               {
                  _context.TblItems.UpdateRange(updatelist);
                  _context.SaveChanges();
               }
            }
         }
         catch(Exception ex)
         {
           throw;
         }
         return Ok();
      }
      [HttpPost]
      [Route("PurchaseFromSupplier")]
      public IActionResult PurchaseFromSupplier(TblPurchase obj)
      {
         try
         {
            TblPurchase tblPurchase = new TblPurchase()
            {
               IntSupplierId = obj.IntSupplierId,
               DtePurchaseDate = obj.DtePurchaseDate,
               IsActive = obj.IsActive
            };
            _context.TblPurchases.Add(tblPurchase);
            _context.SaveChanges();
         }
         catch(Exception ex)
         {
            throw ex;
         }
         return Ok();
      }
      [HttpPost]
      [Route("SaleToCustomer")]
      public IActionResult SaleToCustomer(TblSale obj)
      {
         try
         {
            TblSale tblsale = new TblSale()
            {
               IntCustomerId = obj.IntCustomerId,
               DteSalesDate = obj.DteSalesDate,
               IsActive = obj.IsActive
            };
            _context.TblSales.Add(tblsale);
            _context.SaveChanges();
         }
         catch (Exception ex)
         {
            throw ex;
         }
         return Ok();
      }
   }
}
