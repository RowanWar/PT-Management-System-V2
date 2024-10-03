using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class ClientWeight
{
    public int ClientWeightId { get; set; }

    public int? ClientId { get; set; }

    public decimal? Weight { get; set; }
}
