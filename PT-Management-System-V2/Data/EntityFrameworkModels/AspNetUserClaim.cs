using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.Models;

public partial class AspNetUserClaim
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
