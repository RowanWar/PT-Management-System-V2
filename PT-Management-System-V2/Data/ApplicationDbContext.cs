using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data.EntityFrameworkModels;
//using PT_Management_System_V2.Data.Models;

namespace PT_Management_System_V2.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }


    // Identity DbSets
    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }



    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientMeasurement> ClientMeasurements { get; set; }

    public virtual DbSet<ClientWeight> ClientWeights { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachClient> CoachClients { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<HealthCondition> HealthConditions { get; set; }

    public virtual DbSet<HealthConditionClient> HealthConditionClients { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<SetCategory> SetCategories { get; set; }

    //public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeeklyReport> WeeklyReports { get; set; }

    public virtual DbSet<WeeklyReportImage> WeeklyReportImages { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    public virtual DbSet<WorkoutProgram> WorkoutPrograms { get; set; }

    public virtual DbSet<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }

    public virtual DbSet<WorkoutProgramSchedule> GetWorkoutProgramSchedules { get; set; }

    public virtual DbSet<MuscleGroup> MuscleGroups { get; set; }

    // Modelbuilder is for the purpose of Entity Framework Core to know the relationship and how to map entities to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This calls the Microsoft.Identity base models and is essential to ensure authentication works correctly!
        base.OnModelCreating(modelBuilder);


        // Identity modelbuilders
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.ToTable("AspNetRoles", "public");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });


        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.ToTable("AspNetRoleClaims", "public");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });


        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.ToTable("AspNetUsers", "public");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.AccountActive).HasDefaultValue(true);
            entity.Property(e => e.DateCreated)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles", "identity");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });


        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.ToTable("AspNetUserClaims", "public");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });


        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.ToTable("AspNetUserTokens", "public");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });



        // End of Identity


        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("client");

            entity.HasKey(e => e.ClientId)
                  .HasName("client_pkey");

            entity.Property(e => e.ClientId)
                  .HasColumnName("client_id");

            entity.Property(e => e.UserId)
                  .IsRequired()
                  .HasColumnName("UserId");

            entity.Property(e => e.ApplicationUserId)
                  .HasColumnName("ApplicationUserId");

            // Foreign Key relationship with WorkoutProgram
            entity.HasOne(c => c.WorkoutProgram)
                  .WithMany(wp => wp.Clients)
                  .HasForeignKey(c => c.WorkoutProgramId)
                  .OnDelete(DeleteBehavior.SetNull); // Set WorkoutProgramId to null if the program is deleted

            // Foreign Key relationship with AspNetUsers (UserId)
            entity.HasOne(d => d.User)
                  .WithMany() // No navigation property on AspNetUsers
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_client_AspNetUsers_UserId");

            // Foreign Key relationship with AspNetUsers (ApplicationUserId)
            entity.HasOne(d => d.User)
                  .WithMany() // No navigation property on AspNetUsers
                  .HasForeignKey(d => d.ApplicationUserId)
                  .HasConstraintName("FK_client_AspNetUsers_ApplicationUserId");

            // Indexes
            entity.HasIndex(e => e.UserId)
                  .HasDatabaseName("IX_client_UserId");

            entity.HasIndex(e => e.ApplicationUserId)
                  .HasDatabaseName("IX_client_ApplicationUserId");
        });


        modelBuilder.Entity<WorkoutProgram>(entity =>
        {
            entity.Property(wp => wp.IsDefault)
                  .HasDefaultValue(false);

            entity.Property(wp => wp.CreatedByUserId)
                  .IsRequired(false);

            // Foreign Key relationship for CreatedByUserId with AspNetUsers (User who created the workout program)
            entity.HasOne<AspNetUser>()
                  .WithMany() // No navigation property needed on AspNetUsers
                  .HasForeignKey(wp => wp.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict) // Prevent deletion of user if associated with a WorkoutProgram
                  .HasConstraintName("FK_WorkoutProgram_AspNetUsers_CreatedByUserId");
        });


        modelBuilder.Entity<WorkoutProgramExercise>()
            .HasKey(wpe => new { wpe.WorkoutProgramId, wpe.ExerciseId });

        modelBuilder.Entity<WorkoutProgramExercise>()
            .HasOne(wpe => wpe.WorkoutProgram)
            .WithMany(wp => wp.WorkoutProgramExercises)
            .HasForeignKey(wpe => wpe.WorkoutProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutProgramExercise>()
            .HasOne(wpe => wpe.Exercise)
            .WithMany(e => e.WorkoutProgramExercises)
            .HasForeignKey(wpe => wpe.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<WorkoutProgramSchedule>()
            .HasOne(ws => ws.WorkoutProgram)
            .WithMany(wp => wp.WorkoutSchedules)
            .HasForeignKey(ws => ws.WorkoutProgramId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<ClientMeasurement>(entity =>
        {
            entity.HasKey(e => e.ClientMeasurementId).HasName("client_measurement_pkey");

            entity.ToTable("client_measurement");

            entity.Property(e => e.ClientMeasurementId).HasColumnName("client_measurement_id");
            entity.Property(e => e.Bicep)
                .HasPrecision(4, 1)
                .HasColumnName("bicep");
            entity.Property(e => e.Chest)
                .HasPrecision(4, 1)
                .HasColumnName("chest");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Hip)
                .HasPrecision(4, 1)
                .HasColumnName("hip");
            entity.Property(e => e.Tricep)
                .HasPrecision(4, 1)
                .HasColumnName("tricep");
            entity.Property(e => e.Waist)
                .HasPrecision(4, 1)
                .HasColumnName("waist");
        });

        modelBuilder.Entity<ClientWeight>(entity =>
        {
            entity.HasKey(e => e.ClientWeightId).HasName("client_weight_pkey");

            entity.ToTable("client_weight");

            entity.Property(e => e.ClientWeightId).HasColumnName("client_weight_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Weight)
                .HasPrecision(4, 1)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.ToTable("coach");

            entity.HasKey(e => e.CoachId)
                  .HasName("coach_pkey");  // Set primary key

            entity.Property(e => e.CoachId)
                  .HasColumnName("coach_id");

            entity.Property(e => e.UserId)
                  .IsRequired()
                  .HasColumnName("user_id");

            // Foreign Key relationship with AspNetUsers
            entity.HasOne(d => d.User)
                  .WithMany() 
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_Coach_AspNetUsers_Id");
        });


        modelBuilder.Entity<CoachClient>(entity =>
        {
            entity.ToTable("coach_client");

            // Composite primary key
            entity.HasKey(e => new { e.CoachId, e.ClientId })
                  .HasName("PK_coach_client");

            entity.Property(e => e.CoachId)
                  .HasColumnName("coach_id");

            entity.Property(e => e.ClientId)
                  .HasColumnName("client_id");

            entity.Property(e => e.MonthlyCharge)
                  .IsRequired()
                  .HasColumnName("monthly_charge");

            entity.Property(e => e.ClientStartDate)
                  .IsRequired()
                  .HasColumnName("client_start_date");

            entity.Property(e => e.ClientEndDate)
                  .HasColumnName("client_end_date");

            // Foreign Key relationship with Coach
            entity.HasOne(d => d.Coach)
                  .WithMany(p => p.CoachClients)  
                  .HasForeignKey(d => d.CoachId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("coach_client_coach_id_fkey");

            // Foreign Key relationship with Client
            entity.HasOne(d => d.Client)
                  .WithMany(p => p.CoachClients)  
                  .HasForeignKey(d => d.ClientId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("coach_client_client_id_fkey");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("exercise_pkey");

            entity.ToTable("exercise");

            entity.HasIndex(e => e.ExerciseName, "exercise_exercise_name_key").IsUnique();

            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(100)
                .HasColumnName("exercise_name");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(e => e.ExerciseMuscleGroup)
              .WithMany(mg => mg.Exercises)
              .HasForeignKey(e => e.MuscleGroupId)
              .OnDelete(DeleteBehavior.Restrict)
              .HasConstraintName("FK_Exercise_MuscleGroup");
        });

        modelBuilder.Entity<MuscleGroup>(entity =>
        {
            entity.ToTable("MuscleGroup");

            entity.HasKey(mg => mg.MuscleGroupId)
                  .HasName("MuscleGroup_pkey");

            entity.Property(mg => mg.MuscleGroupId)
                  .HasColumnName("MuscleGroupId");

            entity.Property(mg => mg.MuscleGroupName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnName("MuscleGroupName");
        });

        modelBuilder.Entity<HealthCondition>(entity =>
        {
            entity.HasKey(e => e.HealthConditionId).HasName("health_condition_pkey");

            entity.ToTable("health_condition");

            entity.Property(e => e.HealthConditionId).HasColumnName("health_condition_id");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.HealthCondition1)
                .HasMaxLength(250)
                .HasColumnName("health_condition");
        });

        modelBuilder.Entity<HealthConditionClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("health_condition_client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.HealthConditionId).HasColumnName("health_condition_id");

            entity.HasOne(d => d.Client).WithMany()
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("health_condition_client_client_id_fkey");

            entity.HasOne(d => d.HealthCondition).WithMany()
                .HasForeignKey(d => d.HealthConditionId)
                .HasConstraintName("health_condition_client_health_condition_id_fkey");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.FilePath)
                .HasMaxLength(300)
                .HasColumnName("file_path");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.SetId).HasName("set_pkey");

            entity.ToTable("set");

            entity.Property(e => e.SetId).HasColumnName("set_id");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.SetCategoryId).HasColumnName("set_category_id");
            entity.Property(e => e.Starttime).HasColumnName("starttime");
            entity.Property(e => e.Weight)
                .HasPrecision(8, 2)
                .HasColumnName("weight");
            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workout_exercise_id");

            entity.HasOne(d => d.SetCategory).WithMany(p => p.Sets)
                .HasForeignKey(d => d.SetCategoryId)
                .HasConstraintName("set_set_category_id_fkey");

            entity.HasOne(d => d.WorkoutExercise).WithMany(p => p.Sets)
                .HasForeignKey(d => d.WorkoutExerciseId)
                .HasConstraintName("set_workout_exercise_id_fkey");
        });

        modelBuilder.Entity<SetCategory>(entity =>
        {
            entity.HasKey(e => e.SetCategoryId).HasName("set_category_pkey");

            entity.ToTable("set_category");

            entity.Property(e => e.SetCategoryId).HasColumnName("set_category_id");
            entity.Property(e => e.SetCategoryType)
                .HasMaxLength(25)
                .HasColumnName("set_category_type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .HasColumnName("mobile_number");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<WeeklyReport>(entity =>
        {
            entity.HasKey(e => e.WeeklyReportId).HasName("weekly_report_pkey");

            entity.ToTable("weekly_report");

            entity.Property(e => e.WeeklyReportId).HasColumnName("weekly_report_id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckInWeight)
                .HasPrecision(4, 1)
                .HasColumnName("check_in_weight");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.ClientId).HasColumnName("client_id");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.WeeklyReports)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("weekly_report_user_id_fkey");
        });

        modelBuilder.Entity<WeeklyReportImage>(entity =>
        {
            entity.HasKey(e => e.WeeklyReportImageId).HasName("weekly_report_image_pkey");

            entity.ToTable("weekly_report_image");

            entity.Property(e => e.WeeklyReportImageId).HasColumnName("weekly_report_image_id");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.WeeklyReportId).HasColumnName("weekly_report_id");

            entity.HasOne(d => d.Image).WithMany(p => p.WeeklyReportImages)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("weekly_report_image_image_id_fkey");

            entity.HasOne(d => d.WeeklyReport).WithMany(p => p.WeeklyReportImages)
                .HasForeignKey(d => d.WeeklyReportId)
                .HasConstraintName("weekly_report_image_weekly_report_id_fkey");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("workout_pkey");

            entity.ToTable("workout");

            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.WorkoutActive).HasColumnName("workout_active");
            entity.Property(e => e.WorkoutDate).HasColumnName("workout_date");

            entity.Property(e => e.WorkoutUserId)
                .IsRequired()
                .HasColumnName("UserId");

            entity.HasOne(d => d.AspNetUser)
                .WithMany(p => p.Workouts)
                .HasPrincipalKey(u => u.Id)
                .HasForeignKey(d => d.WorkoutUserId)
                .HasConstraintName("FK_workout_UserId");


            //entity.Property(e => e.WorkoutUserId)
            //    .HasColumnName("UserId");
            ////.IsRequired(true);

            ////// Foreign Key relationship with AspNetUsers
            ////entity.HasOne(d => d.AspNetUser)
            ////      .WithMany(p => p.Workouts)
            ////      .HasForeignKey(d => d.WkoutUserId)
            ////      .OnDelete(DeleteBehavior.Cascade)
            ////      .HasConstraintName("FK_workout_UserId");

            //entity.HasOne(d => d.AspNetUser)
            //    .WithMany(p => p.Workouts)
            //    .HasPrincipalKey(u => u.Id)
            //    .HasForeignKey(d => d.WorkoutUserId)
            //    .HasConstraintName("FK_workout_UserId");
        });



        string debugModel = modelBuilder.Model.ToDebugString();
        Debug.WriteLine(debugModel);

        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutExerciseId).HasName("workout_exercise_pkey");

            entity.ToTable("workout_exercise");

            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workout_exercise_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("workout_exercise_exercise_id_fkey");

            entity.HasOne(d => d.Workout).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.WorkoutId)
                .HasConstraintName("workout_exercise_workout_id_fkey");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
