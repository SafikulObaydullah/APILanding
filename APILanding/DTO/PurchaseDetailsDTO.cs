using APILanding.Models.Data.Entity;

namespace APILanding.DTO
{
   public class PurchaseDetailsDTO
   {
      public long? IntSupplierId { get; set; }
      public DateTime? DtePurchaseDate { get; set; }
      //public long? IntPurchaseId { get; set; }
		public List<TblPurchaseDetail> tblDetails { get; set; }

		//public long? IntItemId { get; set; }


		//public long? NumItemQuantity { get; set; }

		//    public long? NumUnitPrice { get; set; }
		public bool? IsActive { get; set; }
   }
   
}
