using System;
using System.Collections.Generic;

namespace APILanding.Models.Data.Entity;

public partial class TblPartnerType
{
    public long IntPartnerTypeId { get; set; }

    public string StrPartnerTypeName { get; set; } = null!;

    public bool? IsActive { get; set; }
}
