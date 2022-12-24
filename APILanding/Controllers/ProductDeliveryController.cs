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
         string msg = string.Empty;
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
						
                     if (item.IntItemId == 0)
							{
								var existdata = _context.TblItems.Where(x => x.StrItemName.ToLower() == item.StrItemName.ToLower()).FirstOrDefault();

								if (existdata != null)
								{
									//throw new Exception($"{item.StrItemName} Items already Exists"); 
									isExit = true;
									break;
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
                  msg = "Item Already Exist";

						//return BadRequest(isExit);
						return BadRequest(msg);

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
      public IActionResult PurchaseFromSupplier(List<PurchaseDetailsDTO> obj)
      {
         try
         {
            foreach(var item in obj)
            {
               List<TblPurchase> tblPurchaseslist= new List<TblPurchase>();

					TblPurchase tblPurchase = new TblPurchase()
					{
						IntSupplierId = item.IntSupplierId,
						DtePurchaseDate = item.DtePurchaseDate,
						IsActive = item.IsActive,
					};
					
					//tblPurchaseslist.Add(tblPurchase);
					_context.TblPurchases.Add(tblPurchase);
					_context.SaveChanges();
					long purchaseId = tblPurchase.IntPurchaseId;
					if (purchaseId > 0)
               {
						List<TblPurchaseDetail> tblPurchaseDetailslist = new List<TblPurchaseDetail>();
                  foreach(var detail in item.tblDetails)
                  {
							TblPurchaseDetail purchaseDetail = new TblPurchaseDetail()
							{
								 IntItemId = detail.IntItemId,
								 IntPurchaseId = purchaseId,
								 NumItemQuantity = detail.NumItemQuantity,
								 NumUnitPrice = detail.NumUnitPrice,
								 IsActive = detail.IsActive
							};
							tblPurchaseDetailslist.Add(purchaseDetail);
						}
						_context.TblPurchaseDetails.AddRange(tblPurchaseDetailslist);
						_context.SaveChanges();
					} 
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
            TblSale tblsale = new TblSale()
            {
               IntCustomerId = obj.IntCustomerId,
               DteSalesDate = obj.DteSalesDate,
               IsActive = obj.IsActive
            };
            _context.TblSales.Add(tblsale);
            _context.SaveChanges();
            long OldQty = _context.TblItems.Where(x=>x.IntItemId == obj.IntItemId).Select(y=>y.NumStockQuantity).FirstOrDefault();
            long? qty = 0;

				if (OldQty > 0)
				{
                qty = OldQty - obj.NumItemQuantity;
            }
				TblItem itm = _context.TblItems.Where(x => x.IntItemId == obj.IntItemId).FirstOrDefault();
            //itm.NumStockQuantity = qty;
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
         List< DailyPurchaseDTO> data = (from a in _context.TblPurchases
						  join pur in _context.TblPurchaseDetails on a.IntPurchaseId equals pur.IntPurchaseId
                    join itm in _context.TblItems on pur.IntItemId equals itm.IntItemId
                    where a.DtePurchaseDate == dateTime
                    select new DailyPurchaseDTO
                    {
                     itemId = itm.IntItemId,
                     strItemName = itm.StrItemName,
                     Quantity = pur.NumItemQuantity,
                     UnitPrice = pur.NumUnitPrice,
                     TotalPurchase = _context.TblPurchaseDetails.Where(x=>x.IsActive==true).Select(p=>p.NumItemQuantity * p.NumUnitPrice).Sum(),
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
         List<PurchaseSalseDetailsDTO> list = new List<PurchaseSalseDetailsDTO>();
         List<PurchaseDetailsDTO> purchaseList = (from p in _context.TblPurchases
                                                   join purd in _context.TblPurchaseDetails on p.IntPurchaseId equals purd.IntPurchaseId
                                                   where p.DtePurchaseDate == DayTime
                                                   select new PurchaseDetailsDTO()
                                                   {
                                                       // NumItemQuantity = purd.NumItemQuantity,
																		//NumUnitPrice = purd.NumUnitPrice,

																	}).ToList();

			List<SalesDetailsDTO> salesList = (from S in _context.TblSales
                                join sald in _context.TblSalesDetails on S.IntSalesId equals sald.IntSalesId
                                where S.DteSalesDate == DayTime
                                select new SalesDetailsDTO()
                                {
											  NumItemQuantity = sald.NumItemQuantity, 
										  }).ToList();

         foreach(var purlist in purchaseList)
         {
            foreach(var sal in salesList)
            {
               new PurchaseSalseDetailsDTO()
               {
                  IntSupplierId = purlist.IntSupplierId,
                  DtePurchaseDate = purlist.DtePurchaseDate,
                   //PurchaseNumItemQuantity = purlist.NumItemQuantity,
						//PurchaseNumUnitPrice = purlist.NumUnitPrice,

					};

				}
         }
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
