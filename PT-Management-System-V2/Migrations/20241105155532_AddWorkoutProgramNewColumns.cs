using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutProgramNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "WorkoutPrograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "WorkoutPrograms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPrograms_CreatedByUserId",
                table: "WorkoutPrograms",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgram_AspNetUsers_CreatedByUserId",
                table: "WorkoutPrograms",
                column: "CreatedByUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgram_AspNetUsers_CreatedByUserId",
                table: "WorkoutPrograms");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPrograms_CreatedByUserId",
                table: "WorkoutPrograms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "WorkoutPrograms");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "WorkoutPrograms");
        }
    }
}
