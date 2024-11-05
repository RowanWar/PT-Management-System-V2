using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutProgramTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutProgramId",
                table: "client",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkoutPrograms",
                columns: table => new
                {
                    WorkoutProgramId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramName = table.Column<string>(type: "text", nullable: false),
                    ProgramLength = table.Column<int>(type: "integer", nullable: false),
                    WeeklyFrequency = table.Column<int>(type: "integer", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ProgramType = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPrograms", x => x.WorkoutProgramId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_WorkoutProgramId",
                table: "client",
                column: "WorkoutProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_WorkoutPrograms_WorkoutProgramId",
                table: "client",
                column: "WorkoutProgramId",
                principalTable: "WorkoutPrograms",
                principalColumn: "WorkoutProgramId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_WorkoutPrograms_WorkoutProgramId",
                table: "client");

            migrationBuilder.DropTable(
                name: "WorkoutPrograms");

            migrationBuilder.DropIndex(
                name: "IX_client_WorkoutProgramId",
                table: "client");

            migrationBuilder.DropColumn(
                name: "WorkoutProgramId",
                table: "client");
        }
    }
}
