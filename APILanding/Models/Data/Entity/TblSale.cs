using System;
using System.Collections.Generic;

namespace APILanding.Models.Data.Entity;

public partial class TblSale
{
    public long IntSalesId { get; set; }

    public long? IntCustomerId { get; set; }

    public DateTime? DteSalesDate { get; set; }

    public bool? IsActive { get; set; }
}
