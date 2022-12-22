using System;
using System.Collections.Generic;

namespace APILanding.Models.Data.Entity;

public partial class TblPartner
{
    public long IntPartnerId { get; set; }

    public string? StrPartnerName { get; set; }

    public long? IntPartnerTypeId { get; set; }

    public bool? IsActive { get; set; }
}
