﻿using PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

//namespace PT_Management_System_V2.Data.Models;
namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? AccountActive { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();

    //[ForeignKey("UserIdTest2")]
    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
