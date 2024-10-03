using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class CoachClient
{
    public int? CoachId { get; set; }

    public int? ClientId { get; set; }

    public int MonthlyCharge { get; set; }

    public DateTime ClientStartDate { get; set; }

    public DateTime? ClientEndDate { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Coach? Coach { get; set; }
}
