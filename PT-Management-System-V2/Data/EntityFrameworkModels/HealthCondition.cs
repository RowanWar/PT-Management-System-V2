using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class HealthCondition
{
    public int HealthConditionId { get; set; }

    public string? HealthCondition1 { get; set; }

    public DateTime? DateDeleted { get; set; }
}
