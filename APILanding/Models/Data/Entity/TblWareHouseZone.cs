using System;
using System.Collections.Generic;

namespace APILanding.Models.Data.Entity;

public partial class TblWareHouseZone
{
    public long IntAutoId { get; set; }

    public int? IntShipPointId { get; set; }

    public long? IntWhid { get; set; }

    public long? IntTransportZoneId { get; set; }

    public long? IntAccountId { get; set; }

    public long? IntBusinessUnitId { get; set; }

    public bool? IsActive { get; set; }

    public long? IntRouteId { get; set; }
}
