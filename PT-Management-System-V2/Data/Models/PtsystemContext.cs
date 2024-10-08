//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace PT_Management_System_V2.Data.Models;

//public partial class PtsystemContext : DbContext
//{
//    public PtsystemContext()
//    {
//    }

//    public PtsystemContext(DbContextOptions<PtsystemContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }


//    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }


//    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

//    public virtual DbSet<AspNetUser> AspNetUsers1 { get; set; }

//    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }


//    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }


//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Database=ptsystem;Username=postgres;Password=BeBetter30");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<AspNetRole>(entity =>
//        {
//            entity.ToTable("AspNetRoles", "identity");

//            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

//            entity.Property(e => e.Name).HasMaxLength(256);
//            entity.Property(e => e.NormalizedName).HasMaxLength(256);
//        });


//        modelBuilder.Entity<AspNetRoleClaim>(entity =>
//        {
//            entity.ToTable("AspNetRoleClaims", "identity");

//            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

//            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
//        });


//        modelBuilder.Entity<AspNetUser>(entity =>
//        {
//            entity.ToTable("AspNetUsers", "identity");

//            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

//            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

//            entity.Property(e => e.Email).HasMaxLength(256);
//            entity.Property(e => e.Initials).HasMaxLength(5);
//            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
//            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
//            entity.Property(e => e.UserName).HasMaxLength(256);

//            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
//                .UsingEntity<Dictionary<string, object>>(
//                    "AspNetUserRole",
//                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
//                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
//                    j =>
//                    {
//                        j.HasKey("UserId", "RoleId");
//                        j.ToTable("AspNetUserRoles", "identity");
//                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
//                    });
//        });

//        //modelBuilder.Entity<AspNetUser>(entity =>
//        //{
//        //    entity.ToTable("AspNetUsers");

//        //    entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

//        //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

//        //    entity.Property(e => e.Email).HasMaxLength(256);
//        //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
//        //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
//        //    entity.Property(e => e.UserName).HasMaxLength(256);

//        //    entity.HasMany(d => d.Roles).WithMany(p => p.Users)
//        //        .UsingEntity<Dictionary<string, object>>(
//        //            "AspNetUserRole1",
//        //            r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
//        //            l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
//        //            j =>
//        //            {
//        //                j.HasKey("UserId", "RoleId");
//        //                j.ToTable("AspNetUserRoles");
//        //                j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
//        //            });
//        //});

//        modelBuilder.Entity<AspNetUserClaim>(entity =>
//        {
//            entity.ToTable("AspNetUserClaims", "identity");

//            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
//        });


//        modelBuilder.Entity<AspNetUserToken>(entity =>
//        {
//            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

//            entity.ToTable("AspNetUserTokens", "identity");

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
//        });


//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
