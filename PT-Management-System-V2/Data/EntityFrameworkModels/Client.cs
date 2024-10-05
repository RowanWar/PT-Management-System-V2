using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Client
{
    public int ClientId { get; set; }

    // Foreign key to AspNetUser (Id)
    //public string UserId { get; set; }

    public bool? ContactByPhone { get; set; }

    public bool? ContactByEmail { get; set; }

    public bool? Referred { get; set; }

    public string? Referral { get; set; }

    public virtual ICollection<Coach> Coaches { get; set; } = new List<Coach>();
}
