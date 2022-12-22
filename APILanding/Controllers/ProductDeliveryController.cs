using APILanding.DTO;
using APILanding.Models.Data;
using APILanding.Models.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using System;
using System.Runtime.InteropServices;

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
      #region ===== 01 Create partner type [Customer, Supplier] ====== 
      [HttpPost]
      [Route("CreatePartnerType")]
      public IActionResult CreatePartnerType(TblPartnerType tblPartnerType)
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
         catch (Exception ex)
         {
            throw;
         }
         return Ok();
      }
      #region ===== 2# Create some customer and supplier [Single] ====== 
      [HttpPost]
      [Route("CreatePartner")]
      public IActionResult CreatePartner(TblPartner obj)
      {
         TblPartner tblPartner = new TblPartner()
         {
            StrPartnerName = obj.StrPartnerName,
            IntPartnerTypeId = obj.IntPartnerTypeId,
            IsActive = obj.IsActive
         };
         return Ok(obj);
      }
      #endregion =========================
      #endregion =========================

      #region ===== 3# Create Some items [List of item, don’t allow duplicates] 4# Edit some items [List of item, don’t allow duplicates]======

      [HttpPost]
      [Route("CreateListofItem")]
      public IActionResult CreateListofItem(List<TblItem> obj)
      {
         string msg = "";
         try
         {
            bool isExit = false;
            //var existdata1 = _context.TblItems.Where(x => x.StrItemName.ToLower() == obj.Select(o => o.StrItemName.ToLower()).ToList());
            List<TblItem> itemlist = new List<TblItem>();
            List<TblItem> updatelist = new List<TblItem>();
            
            if (obj.Count() > 0)
            {
               foreach (var item in obj)
               {
                  var existdata = _context.TblItems.Where(x => x.StrItemName.ToLower() == item.StrItemName.ToLower()).FirstOrDefault();
                  

                  
                  if (existdata != null)
                  {
                     //throw new Exception($"{item.StrItemName} Items already Exists"); 
                     isExit = true;
                     break;
                  }
                  else
                  {
                     if (item.IntItemId == 0)
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
                        var updata = _context.TblItems.Where(x => x.StrItemName.ToLower() == obj.Select(a => a.StrItemName.ToLower()).FirstOrDefault()).Count();
                        if (updata > 0) throw new Exception($"{item.StrItemName}Items already Exists");
                        TblItem tbl = _context.TblItems.Where(x => x.IntItemId == item.IntItemId).FirstOrDefault();
                        tbl.StrItemName = item.StrItemName;
                        tbl.NumStockQuantity = item.NumStockQuantity;
                        tbl.IsActive = item.IsActive;

                        updatelist.Add(tbl);
                     }
                  }
               }
               if(isExit == false)
               {
                  if (itemlist.Count() > 0)
                  {
                     _context.TblItems.AddRange(itemlist);
                     _context.SaveChanges();
                  }

                  if (updatelist.Count() > 0)
                  {
                     _context.TblItems.UpdateRange(updatelist);
                     _context.SaveChanges();
                  }  
                  return Ok("Created Successfully"); 
               }
               else
               {

                  return BadRequest(isExit);
               }

            }
            else
            {
               return BadRequest();
            }


         }
         catch (Exception ex)
         {
            throw ex;
         } 
      }
      #endregion =========================



      #region =====  5# Purchase Some item from a supplier ====== 
      [HttpPost]
      [Route("PurchaseFromSupplier")]
      public IActionResult PurchaseFromSupplier(PurchaseDetailsDTO obj)
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
            TblPurchaseDetail purchaseDetail = new TblPurchaseDetail()
            {
               IntItemId = obj.IntItemId,
               IntPurchaseId = obj.IntPurchaseId,
               NumItemQuantity = obj.NumItemQuantity,
               NumUnitPrice = obj.NumUnitPrice,
               IsActive = obj.IsActive
            };
            _context.Add(purchaseDetail);
            _context.SaveChanges(); 

         }
         catch (Exception ex)
         {
            throw ex;
         }
         return Ok();
      }
      #endregion =========================

      #region ===== 6# Sale some item to a customer [ Check stock while selling items] ====== 
      [HttpPost]
      [Route("SaleToCustomer")]
      public IActionResult SaleToCustomer(SalesDetailsDTO obj)
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
            TblSalesDetail tblSalesDetail = new TblSalesDetail()
            {
               IntItemId = obj.IntItemId,
               IntSalesId = obj.IntSalesId,
               NumItemQuantity = obj.NumItemQuantity,
               NumUnitPrice = obj.NumUnitPrice,
               IsActive = obj.IsActive
            };
            _context.Add(tblSalesDetail);
            _context.SaveChanges();
         }
         catch (Exception ex)
         {
            throw ex;
         }
         return Ok();
      }
      #endregion =========================

      #region ===== 7# Find item wise Daily Purchase report [Define your own fields for report] ====== 
      [HttpGet]
      [Route("DailyPurchaseReport")]
      public IActionResult DailyPurchaseReport(DateTime dateTime)
      {
         var data = from a in _context.TblPurchaseDetails
                    join pur in _context.TblPurchases on a.IntPurchaseId equals pur.IntPurchaseId
                    join itm in _context.TblItems on a.IntItemId equals itm.IntItemId
                    where pur.DtePurchaseDate == dateTime
                    select new DailyPurchaseDTO
                    {
                     itemId = itm.IntItemId,
                     strItemName = itm.StrItemName,
                     Quantity = a.NumItemQuantity,
                     UnitPrice = a.NumUnitPrice,
                     TotalPurchase = _context.TblPurchaseDetails.Where(x=>x.IsActive==true).Select(p=>p.NumItemQuantity * p.NumUnitPrice).Sum(),
                     
                     };
         return Ok(data);
      }
      #endregion =========================

      #region ===== 8# Find item wise Monthly Sales report [Define your own fields for report] ====== 
      [HttpGet]
      [Route("MonthlyWiseSalesReport")]
      public IActionResult MonthlyWiseSalesReport(DateTime startDate,DateTime ToDate)
      {
         var data = from a in _context.TblSalesDetails
                    join pur in _context.TblSales on a.IntSalesId equals pur.IntSalesId
                    join itm in _context.TblItems on a.IntItemId equals itm.IntItemId
                    where (pur.DteSalesDate >= startDate && pur.DteSalesDate <= ToDate)
                    select new MonthlyWiseSalesReportDTO
                    {
                       itemId = itm.IntItemId,
                       strItemName = itm.StrItemName,
                       Quantity = a.NumItemQuantity,
                       UnitPrice = a.NumUnitPrice,
                       TotalPurchase = _context.TblPurchaseDetails.Where(x => x.IsActive == true).Select(p => p.NumItemQuantity * p.NumUnitPrice).Sum(),
                       
                    };
         return Ok(data);
      }
      #endregion =========================

      #region ===== 09# Find item wise Daily Purchase vs Sales Report ====== 
      [HttpGet]
      [Route("DailywisePurchasevsSalesReport")]
      public IActionResult DailywisePurchasevsSalesReport(DateTime DayTime)
      {
         List<DailywisePurchasevsSalesReportDTO> dt = (from a in _context.TblPurchaseDetails

                   join itm in _context.TblItems on a.IntItemId equals itm.IntItemId
                   join pur in _context.TblPurchases on a.IntPurchaseId equals pur.IntPurchaseId
                   //join salDet in _context.TblSalesDetails on a.IntItemId equals salDet.IntItemId
                   //join sal in _context.TblSales on salDet.IntSalesId equals sal.IntSalesId
                   where a.IsActive == true && pur.DtePurchaseDate == DayTime
                   select new DailywisePurchasevsSalesReportDTO()
                   {
                      ItemId= a.IntItemId,   
                      strItemName = itm.StrItemName,
                      PurchaseQuantity= a.NumItemQuantity,
                      PurchasePrice = a.NumUnitPrice,
                      //SalesQuantity = salDet.NumItemQuantity,
                      //SalesPrice = salDet.NumUnitPrice,
                      TotalPurchase = _context.TblPurchaseDetails.Where(x => x.IsActive == true).Select(p => p.NumItemQuantity * p.NumUnitPrice).Sum(),
                      TotalSales = _context.TblSalesDetails.Where(s=>s.IsActive == true).Select(ss=>ss.NumItemQuantity * ss.NumUnitPrice).Sum(),   
                      
                   }).ToList();
         // List<PurchaseSalseDTO> list = new List<PurchaseSalseDTO>();
         //List<TblPurchase> data = (from p in _context.TblPurchases
         //                          where p.DtePurchaseDate == DayTime
         //                          select p).ToList();

         //List<TblSale> data1 = (from S in _context.TblSales
         //                       where S.DteSalesDate == DayTime
         //                       select S).ToList();

         //data.AddRange(data1);
         return Ok(dt);
      }
      #endregion =========================

      #region ===== 10# Find Report with given column (Monthname, year, total purchase amount, total sales amount, profit/loss status) =======

      [HttpGet]
      [Route("LandingReport")]
      public IActionResult LandingReport()
      {
         var totalPurchase = (from ps in _context.TblPurchaseDetails
                              join p in _context.TblPurchases on ps.IntPurchaseId equals p.IntPurchaseId
                              where ps.IsActive == true
                              select ps.NumItemQuantity * ps.NumUnitPrice).Sum();
         var totalSales = (from ds in _context.TblSalesDetails
                           join s in _context.TblSales on ds.IntSalesId equals s.IntSalesId
                           where ds.IsActive == true
                           select ds.NumItemQuantity * ds.NumUnitPrice).Sum();
         string loss = "";
         string profit = "";
         if (totalPurchase > totalSales)
         {
            loss = "Item Sale is loss";
         }
         else
         {
            profit = "Item purchase is Profit";
         }
         var result = from ps in _context.TblPurchaseDetails
                      join p in _context.TblPurchases on ps.IntPurchaseId equals p.IntPurchaseId
                      where ps.IsActive == true
                      select new LandingReportDTO()
                      {
                         Monthname = DateTime.Now.ToString("MMMM"),
                         Year = DateTime.Now.ToString("yyyy"),
                         TotalPurchaseAmount = totalPurchase,
                         TotalSalesAmount = totalSales,
                         LossStatus = loss,
                         ProfitSatus = profit,
                      };

         return Ok(result);
      }
      #endregion =========================

   }
}
