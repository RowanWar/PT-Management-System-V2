using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class HealthConditionClient
{
    public int? ClientId { get; set; }

    public int? HealthConditionId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual HealthCondition? HealthCondition { get; set; }
}
