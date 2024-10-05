using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    AccountActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            // Here too
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contact_by_phone = table.Column<bool>(type: "boolean", nullable: true),
                    contact_by_email = table.Column<bool>(type: "boolean", nullable: true),
                    referred = table.Column<bool>(type: "boolean", nullable: true),
                    referral = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_pkey", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "client_measurement",
                columns: table => new
                {
                    client_measurement_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    chest = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    hip = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    waist = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    bicep = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    tricep = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_measurement_pkey", x => x.client_measurement_id);
                });

            migrationBuilder.CreateTable(
                name: "client_weight",
                columns: table => new
                {
                    client_weight_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    weight = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_weight_pkey", x => x.client_weight_id);
                });

            migrationBuilder.CreateTable(
                name: "exercise",
                columns: table => new
                {
                    exercise_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exercise_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    muscle_group = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("exercise_pkey", x => x.exercise_id);
                });

            migrationBuilder.CreateTable(
                name: "health_condition",
                columns: table => new
                {
                    health_condition_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    health_condition = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("health_condition_pkey", x => x.health_condition_id);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_pkey", x => x.image_id);
                });

            migrationBuilder.CreateTable(
                name: "set_category",
                columns: table => new
                {
                    set_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    set_category_type = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("set_category_pkey", x => x.set_category_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    mobile_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coach",
                columns: table => new
                {
                    coach_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coach_client_id = table.Column<int>(type: "integer", nullable: true),
                    coach_profile_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    coach_qualifications = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("coach_pkey", x => x.coach_id);
                    table.ForeignKey(
                        name: "coach_coach_client_id_fkey",
                        column: x => x.coach_client_id,
                        principalTable: "client",
                        principalColumn: "client_id");
                });

            migrationBuilder.CreateTable(
                name: "health_condition_client",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    health_condition_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "health_condition_client_client_id_fkey",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id");
                    table.ForeignKey(
                        name: "health_condition_client_health_condition_id_fkey",
                        column: x => x.health_condition_id,
                        principalTable: "health_condition",
                        principalColumn: "health_condition_id");
                });

            migrationBuilder.CreateTable(
                name: "weekly_report",
                columns: table => new
                {
                    weekly_report_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    check_in_weight = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("weekly_report_pkey", x => x.weekly_report_id);
                    table.ForeignKey(
                        name: "weekly_report_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "workout",
                columns: table => new
                {
                    workout_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    workout_date = table.Column<DateOnly>(type: "date", nullable: false),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    workout_active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workout_pkey", x => x.workout_id);
                    table.ForeignKey(
                        name: "workout_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "coach_client",
                columns: table => new
                {
                    coach_id = table.Column<int>(type: "integer", nullable: true),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    monthly_charge = table.Column<int>(type: "integer", nullable: false),
                    client_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    client_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "coach_client_client_id_fkey",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id");
                    table.ForeignKey(
                        name: "coach_client_coach_id_fkey",
                        column: x => x.coach_id,
                        principalTable: "coach",
                        principalColumn: "coach_id");
                });

            migrationBuilder.CreateTable(
                name: "weekly_report_image",
                columns: table => new
                {
                    weekly_report_image_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    weekly_report_id = table.Column<int>(type: "integer", nullable: true),
                    image_id = table.Column<int>(type: "integer", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("weekly_report_image_pkey", x => x.weekly_report_image_id);
                    table.ForeignKey(
                        name: "weekly_report_image_image_id_fkey",
                        column: x => x.image_id,
                        principalTable: "image",
                        principalColumn: "image_id");
                    table.ForeignKey(
                        name: "weekly_report_image_weekly_report_id_fkey",
                        column: x => x.weekly_report_id,
                        principalTable: "weekly_report",
                        principalColumn: "weekly_report_id");
                });

            migrationBuilder.CreateTable(
                name: "workout_exercise",
                columns: table => new
                {
                    workout_exercise_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workout_id = table.Column<int>(type: "integer", nullable: true),
                    exercise_id = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("workout_exercise_pkey", x => x.workout_exercise_id);
                    table.ForeignKey(
                        name: "workout_exercise_exercise_id_fkey",
                        column: x => x.exercise_id,
                        principalTable: "exercise",
                        principalColumn: "exercise_id");
                    table.ForeignKey(
                        name: "workout_exercise_workout_id_fkey",
                        column: x => x.workout_id,
                        principalTable: "workout",
                        principalColumn: "workout_id");
                });

            migrationBuilder.CreateTable(
                name: "set",
                columns: table => new
                {
                    set_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workout_exercise_id = table.Column<int>(type: "integer", nullable: true),
                    set_category_id = table.Column<int>(type: "integer", nullable: true),
                    reps = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    starttime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    endtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("set_pkey", x => x.set_id);
                    table.ForeignKey(
                        name: "set_set_category_id_fkey",
                        column: x => x.set_category_id,
                        principalTable: "set_category",
                        principalColumn: "set_category_id");
                    table.ForeignKey(
                        name: "set_workout_exercise_id_fkey",
                        column: x => x.workout_exercise_id,
                        principalTable: "workout_exercise",
                        principalColumn: "workout_exercise_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            // I commented this all out so it would initialy run an InitialCreate
            migrationBuilder.CreateIndex(
                name: "IX_coach_coach_client_id",
                table: "coach",
                column: "coach_client_id");

            migrationBuilder.CreateIndex(
                name: "IX_coach_client_client_id",
                table: "coach_client",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_coach_client_coach_id",
                table: "coach_client",
                column: "coach_id");

            migrationBuilder.CreateIndex(
                name: "exercise_exercise_name_key",
                table: "exercise",
                column: "exercise_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_health_condition_client_client_id",
                table: "health_condition_client",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_health_condition_client_health_condition_id",
                table: "health_condition_client",
                column: "health_condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_set_set_category_id",
                table: "set",
                column: "set_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_set_workout_exercise_id",
                table: "set",
                column: "workout_exercise_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_report_user_id",
                table: "weekly_report",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_weekly_report_image_image_id",
                table: "weekly_report_image",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_weekly_report_image_weekly_report_id",
                table: "weekly_report_image",
                column: "weekly_report_id");

            migrationBuilder.CreateIndex(
                name: "IX_workout_user_id",
                table: "workout",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_workout_exercise_exercise_id",
                table: "workout_exercise",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_workout_exercise_workout_id",
                table: "workout_exercise",
                column: "workout_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "client_measurement");

            migrationBuilder.DropTable(
                name: "client_weight");

            migrationBuilder.DropTable(
                name: "coach_client");

            migrationBuilder.DropTable(
                name: "health_condition_client");

            migrationBuilder.DropTable(
                name: "set");

            migrationBuilder.DropTable(
                name: "weekly_report_image");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "coach");

            migrationBuilder.DropTable(
                name: "health_condition");

            migrationBuilder.DropTable(
                name: "set_category");

            migrationBuilder.DropTable(
                name: "workout_exercise");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "weekly_report");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "exercise");

            migrationBuilder.DropTable(
                name: "workout");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
