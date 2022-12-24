﻿using APILanding.Models.Data.Entity;

namespace APILanding.DTO
{
   public class PurchaseDetailsDTO
   {
      public long? IntSupplierId { get; set; }
      public DateTime? DtePurchaseDate { get; set; }
		public List<TblPurchaseDetail> tblDetails { get; set; }

		public bool? IsActive { get; set; }
   }
   
}
