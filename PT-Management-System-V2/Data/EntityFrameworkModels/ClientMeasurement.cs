using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class ClientMeasurement
{
    public int ClientMeasurementId { get; set; }

    public int? ClientId { get; set; }

    public decimal? Chest { get; set; }

    public decimal? Hip { get; set; }

    public decimal? Waist { get; set; }

    public decimal? Bicep { get; set; }

    public decimal? Tricep { get; set; }
}
