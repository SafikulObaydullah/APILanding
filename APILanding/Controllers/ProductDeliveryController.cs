using APILanding.DTO;
using APILanding.Models.Data;
using APILanding.Models.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Numerics;
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
      #endregion
      #endregion

      #region ===== 3# Create Some items [List of item, don’t allow duplicates] 4# Edit some items [List of item, don’t allow duplicates]======

      [HttpPost]
      [Route("CreateListofItem")]
      public IActionResult CreateListofItem(List<TblItem> obj)
      {
         string msg = string.Empty;
         //MessageHelper response = new MessageHelper();   
         try
         {
            bool isExit = false;
            //var existdata1 = _context.TblItems.Where(x => x.StrItemName.ToLower() == obj.Select(o => o.StrItemName.ToLower()).ToList());
            List<TblItem> itemlist = new List<TblItem>();
            List<TblItem> updatelist = new List<TblItem>();
            List<TblItem> duplicate  = new List<TblItem>();
            List<MessageHelper> list = new List<MessageHelper>();
            if (obj.Count() > 0)
            {
               long count = 0;
               msg = "";
               foreach (var item in obj)
               {
						
                     if (item.IntItemId == 0)
							{
                        /// loop use 
								var existdata = _context.TblItems.Where(x => x.StrItemName.ToLower() == item.StrItemName.ToLower()).FirstOrDefault();
                        
                        count++;
                     
                        duplicate.Add(existdata);
                        
                        if (existdata != null)
								{
									//throw new Exception($"{item.StrItemName} Items already Exists"); 
									isExit = true;
                           continue;
								}
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
                     var updata = _context.TblItems.Where(x => x.IntItemId != item.IntItemId && x.StrItemName.ToLower() == item.StrItemName.ToLower()).ToList().Count();

                     if (updata > 0) throw new Exception("Items already Exists");
                        TblItem tbl = _context.TblItems.Where(x => x.IntItemId == item.IntItemId).FirstOrDefault();
                        tbl.StrItemName = item.StrItemName;
                        tbl.NumStockQuantity = item.NumStockQuantity;
                        tbl.IsActive = item.IsActive;
                        updatelist.Add(tbl);
                     }
                  
               }
               msg = msg  + "Total Exists Count: " + count.ToString();
               duplicate.ForEach(x =>
               {
                  msg =  msg + " " + x.StrItemName + ",";
               });
               msg = msg.Remove(msg.Length - 1, 1);


               if (isExit == false)
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
                  
                  foreach(var dup in duplicate)
                  {
                     //msg = $"{count} {dup.StrItemName} Items Already Exist";
                     MessageHelper response = new MessageHelper()
                     {
                        Name = dup.StrItemName,
                        //Count = count,
                        Message = "Items Already Exists",
                     };
                     list.Add(response);
                  }
                  //return BadRequest(isExit);
                  return BadRequest(msg);
					}

            }
            else
            {
               return BadRequest();
            }

            //return msg;
         }
         catch (Exception ex)
         {
            throw ex;
         } 
      }
      #endregion =========================

     
      //public IActionResult DailywisePurchasevsSalesReport(DateTime DayTime)
      //{
      //   List<PurchaseDetailsDTO> purchaseList = (from a in _context.TblPurchases
                                                  
      //                                    where a.IsActive == true
      //                                    && a.DtePurchaseDate== DayTime
      //                                    select new PurchaseDetailsDTO()
      //                                    {
      //                                       IntSupplierId = a.IntSupplierId,
      //                                       tblDetails = (from pur in _context.TblPurchaseDetails
      //                                                     join itm in _context.TblItems on pur.IntItemId equals itm.IntItemId
      //                                                     where pur.IsActive == true && itm.IntItemId == pur.IntItemId
                                                           
      //                                                     select new PurchaseDtlDTO()
      //                                                     {
      //                                                        NumItemQuantity = pur.NumItemQuantity,
      //                                                        NumUnitPrice = pur.NumUnitPrice,
      //                                                     }).ToList(),

      //                                    }).ToList();
      //      List<SalesDetailsDTO> salesList = (from S in _context.TblSales
                                               
      //                                         join sald in _context.TblSalesDetails on S.IntSalesId equals sald.IntSalesId
      //                                         join itms in _context.TblItems on sald.IntItemId equals itms.IntItemId
      //                                         where S.DteSalesDate == DayTime 
      //                                         && itms.IntItemId == sald.IntItemId

      //                                         select new SalesDetailsDTO()
      //                                         {
      //                                            NumItemQuantity = sald.NumItemQuantity,
      //                                            NumUnitPrice = sald.NumUnitPrice,
      //                                         }).ToList();

      //   var datalist = purchaseList.Join(
      //                                   salesList,
      //                                   a=>a.IntSupplierId,
      //                                   b=>b.IntCustomerId,
      //                                   (a,b)=> new {a.IntSupplierId,b.NumItemQuantity}).ToList();

      //   return Ok(datalist);
      //}

      #region =====  5# Purchase Some item from a supplier ====== 
      [HttpPost]
      [Route("PurchaseFromSupplier")]
      public IActionResult PurchaseFromSupplier(PurchaseDetailsDTO obj)
      {
         try
         {
            
               List<TblPurchase> tblPurchaseslist= new List<TblPurchase>();

					TblPurchase tblPurchase = new TblPurchase()
					{
						IntSupplierId = obj.IntSupplierId,
						DtePurchaseDate = obj.DtePurchaseDate,
						IsActive = obj.IsActive,
					};
					_context.TblPurchases.Add(tblPurchase);
					_context.SaveChanges();
					long purchaseId = tblPurchase.IntPurchaseId;

            List<TblItem> tblItemslist = new List<TblItem>();
            foreach (var item in obj.tblDetails)
            {
               if (purchaseId > 0)
               {
                  TblPurchaseDetail purchaseDetail = new TblPurchaseDetail()
                  {
                     IntItemId = item.IntItemId,
                     IntPurchaseId = purchaseId,
                     NumItemQuantity = item.NumItemQuantity,
                     NumUnitPrice = item.NumUnitPrice,
                     IsActive = item.IsActive
                  };
                 
                  _context.TblPurchaseDetails.Add(purchaseDetail);
                  _context.SaveChanges();
               }
               TblItem tbl = _context.TblItems.Where(x => x.IntItemId == item.IntItemId).FirstOrDefault();
               tbl.NumStockQuantity = tbl.NumStockQuantity + item.NumItemQuantity;
               tbl.IsActive = item.IsActive;
               tblItemslist.Add(tbl);
               _context.TblItems.UpdateRange(tblItemslist);
               _context.SaveChanges();
            }

            
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
            long? OldQty = _context.TblItems.Where(x => x.IntItemId == obj.IntItemId).Select(y => y.NumStockQuantity).FirstOrDefault();
            if(OldQty>0)
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
             long? qty = 0;
               qty = OldQty - obj.NumItemQuantity;
			      TblItem itm = _context.TblItems.Where(x => x.IntItemId == obj.IntItemId).FirstOrDefault();
               _context.TblItems.Update(itm);
               _context.SaveChanges();
				TblSalesDetail tblSalesDetail = new TblSalesDetail()
            {
               IntItemId = obj.IntItemId,
               
               IntSalesId = obj.IntSalesId,
               NumItemQuantity = obj.NumItemQuantity,
               NumUnitPrice = obj.NumUnitPrice,
               IsActive = obj.IsActive
            };
            _context.TblSalesDetails.Add(tblSalesDetail);
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

         List<DailyPurchaseDTO> data = (from a in _context.TblPurchases
                                        join pur in _context.TblPurchaseDetails on a.IntPurchaseId equals pur.IntPurchaseId
                                        join itm in _context.TblItems on pur.IntItemId equals itm.IntItemId
                                        where a.DtePurchaseDate.Value.Date == dateTime.Date && a.IsActive == true
                                        select new DailyPurchaseDTO
                                        {
                                           itemId = itm.IntItemId,
                                           strItemName = itm.StrItemName,
                                           Quantity = pur.NumItemQuantity,
                                           UnitPrice = pur.NumUnitPrice,
                                           TotalPurchase = _context.TblPurchaseDetails.Where(x => x.IsActive == true && x.IntItemId == itm.IntItemId).Select(p => p.NumItemQuantity * p.NumUnitPrice).Sum(),
                                          
                                        }).ToList();
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
                    where (pur.DteSalesDate.Value.Month >= startDate.Month && pur.DteSalesDate.Value.Month <= ToDate.Month)
                    && pur.IsActive == true
                    select new MonthlyWiseSalesReportDTO
                    {
                       itemId = itm.IntItemId,
                       strItemName = itm.StrItemName,
                       Quantity = a.NumItemQuantity,
                       UnitPrice = a.NumUnitPrice,
                       TotalPurchase = _context.TblPurchaseDetails.Where(x => x.IsActive == true).Select(p => p.NumItemQuantity * p.NumUnitPrice).Sum(),
                    };
         long? count = data.Count();
         return Ok(data);
      }
      #endregion =========================

      #region ===== 09# Find item wise Daily Purchase vs Sales Report ====== 
      [HttpGet]
      [Route("DailywisePurchasevsSalesReport")]

      public IActionResult DailywisePurchasevsSalesReport(DateTime DayTime)
      {
         List<PurchaseDtlDTO> purchaseList = (from a in _context.TblPurchases
                                              join pur in _context.TblPurchaseDetails on a.IntPurchaseId equals pur.IntPurchaseId
                                              join itm in _context.TblItems on pur.IntItemId equals itm.IntItemId
                                              where a.DtePurchaseDate.Value.Date == DayTime.Date 
                                                    && a.IsActive == true && pur.IsActive == true
                                              select new PurchaseDtlDTO()
                                              {
                                                 IntDetailsId = pur.IntDetailsId,
                                                 IntPurchaseId= pur.IntPurchaseId,
                                                 IsActive= pur.IsActive,
                                                 IntItemId = itm.IntItemId,
                                                 StrItemName = itm.StrItemName,
                                                 NumItemQuantity = pur.NumItemQuantity,   
                                                 NumUnitPrice = pur.NumUnitPrice,
                                                 
                                              }).GroupBy(y=> new
                                              {
                                                 y.IntItemId,
                                                 y.StrItemName
                                              }).Select(x=> new PurchaseDtlDTO()
                                              {
                                                 IntItemId = x.Key.IntItemId,
                                                 StrItemName = x.Key.StrItemName,
                                                 NumItemQuantity = x.Sum(w=>w.NumItemQuantity),
                                              }).ToList();
         var Presults = from pur in purchaseList
                        group pur by new { pur.IntItemId, pur.StrItemName } into pp
                        select new
                        {
                           IntItemId = pp.Key.IntItemId,
                           StrItemName = pp.Key.StrItemName,
                           ItemQuantity = pp.Sum(x => x.NumItemQuantity),
                        };

         List<SalesDetailsDTO> salesList = (from a in _context.TblSales
                                            join sal in _context.TblSalesDetails on a.IntSalesId equals sal.IntSalesId
                                            join itm in _context.TblItems on sal.IntItemId equals itm.IntItemId
                                            where a.DteSalesDate.Value.Date == DayTime.Date 
                                            && a.IsActive == true && sal.IsActive== true  
                                            select new SalesDetailsDTO()
                                            {
                                               IntSalesId = a.IntSalesId,
                                               IntCustomerId = a.IntCustomerId,
                                               DteSalesDate= a.DteSalesDate,
                                               IsActive = a.IsActive,
                                               
                                               IntItemId = itm.IntItemId,
                                               NumItemQuantity = sal.NumItemQuantity,
                                               strItemName = itm.StrItemName
                                               //NumUnitPrice = _context.TblSalesDetails.Where(y=>y.IntItemId == sal.IntItemId).Select(w=>w.NumUnitPrice).Sum(),
                                            }).GroupBy(y=> new
                                            {
                                               y.IntItemId,
                                               y.strItemName
                                            }).Select(x=> new SalesDetailsDTO()
                                            {
                                               IntItemId = x.Key.IntItemId,
                                               strItemName = x.Key.strItemName,
                                               NumItemQuantity = x.Sum(w=>w.NumItemQuantity),
                                            }).ToList();
         var salesData = from s in salesList
                         group s by new { s.IntItemId, s.strItemName } into gg
                         select new
                         {
                            IntItemId = gg.Key.IntItemId,
                            StrItemName = gg.Key.strItemName,
                            ItemQuantity = gg.Sum(x => x.NumItemQuantity),
                         };
         var res = from i in Presults
                   join j in salesData on i.IntItemId equals j.IntItemId
                   where i.IntItemId == i.IntItemId
                   select new PurchaseVSsalesDTO()
                   {
                      strItemName = i.StrItemName,
                      PurchaseQuantity = i.ItemQuantity,
                      SalesQuantity = j.ItemQuantity
                   };

         var response = (from i in purchaseList
                         join j in salesList on i.IntItemId equals j.IntItemId
                         //where /*j.IntItemId == i.IntItemId &&*/ i.IsActive == true && j.IsActive == true
                         select new PurchaseVSsalesDTO()
                         {
                            strItemName = i.StrItemName,
                            PurchaseQuantity = i.NumItemQuantity,
                            SalesQuantity = j.NumItemQuantity
                         }).GroupBy(x => x.strItemName).ToList();
         return Ok(res);
      }
      #endregion

      #region ===== 10# Find Report with given column (Monthname, year, total purchase amount, total sales amount, profit/loss status) =======

      [HttpGet]
      [Route("LandingReport")]
      public IActionResult LandingReport(int Month,int Year )
      {
         var totalPurchase = (from ps in _context.TblPurchases
                              join p in _context.TblPurchaseDetails on ps.IntPurchaseId equals p.IntPurchaseId
                              where ps.IsActive == true && p.IsActive == true
                              && ps.DtePurchaseDate.Value.Month == Month
                              && ps.DtePurchaseDate.Value.Year == Year
                              select p.NumItemQuantity * p.NumUnitPrice).Sum();
         var totalSales = (from ds in _context.TblSales
                           join s in _context.TblSalesDetails on ds.IntSalesId equals s.IntSalesId
                           where ds.IsActive == true 
                           && ds.DteSalesDate.Value.Month == Month
                           && ds.DteSalesDate.Value.Month == Year
                           select s.NumItemQuantity * s.NumUnitPrice).Sum();

         string status = "";
         if (totalPurchase > totalSales)
         {
            status = "Item Sale is loss";
         }
         else
         {
            status = "Item purchase is Profit";
         }

         var result = new LandingReportDTO
         {
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(Month)),
            Year = Year,
            TotalPurchaseAmount = totalPurchase,
            TotalSalesAmount = totalSales,
            Status = status
         };

         return Ok(result);
      }
		#endregion 


	}
}
