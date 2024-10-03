using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class SetCategory
{
    public int SetCategoryId { get; set; }

    public string? SetCategoryType { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
