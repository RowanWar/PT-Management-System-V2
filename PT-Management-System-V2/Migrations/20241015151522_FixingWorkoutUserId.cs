using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class FixingWorkoutUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workout_UserId",
                table: "workout");

            migrationBuilder.DropIndex(
                name: "IX_workout_UserId",
                table: "workout");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "workout");

            migrationBuilder.CreateIndex(
                name: "IX_workout_UserId",
                table: "workout",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_workout_UserId",
                table: "workout",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workout_UserId",
                table: "workout");

            migrationBuilder.DropIndex(
                name: "IX_workout_UserId",
                table: "workout");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                table: "workout",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_workout_AspNetUserId",
                table: "workout",
                column: "AspNetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_workout_AspNetUsers_AspNetUserId",
                table: "workout",
                column: "AspNetUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
