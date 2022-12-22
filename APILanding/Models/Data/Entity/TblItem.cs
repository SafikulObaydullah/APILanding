using System;
using System.Collections.Generic;

namespace APILanding.Models.Data.Entity;

public partial class TblItem
{
    public long IntItemId { get; set; }

    public string? StrItemName { get; set; }

    public long? NumStockQuantity { get; set; }

    public bool? IsActive { get; set; }
}
