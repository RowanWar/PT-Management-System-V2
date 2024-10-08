using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class RenameCoachClientModel4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "coach_client_client_id",
                table: "coach_client");

            migrationBuilder.DropForeignKey(
                name: "coach_client_coach_id",
                table: "coach_client");

            migrationBuilder.AddForeignKey(
                name: "coach_client_client_id_fkey",
                table: "coach_client",
                column: "client_id",
                principalTable: "client",
                principalColumn: "client_id");

            migrationBuilder.AddForeignKey(
                name: "coach_client_coach_id_fkey",
                table: "coach_client",
                column: "coach_id",
                principalTable: "coach",
                principalColumn: "coach_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "coach_client_client_id_fkey",
                table: "coach_client");

            migrationBuilder.DropForeignKey(
                name: "coach_client_coach_id_fkey",
                table: "coach_client");

            migrationBuilder.AddForeignKey(
                name: "coach_client_client_id",
                table: "coach_client",
                column: "client_id",
                principalTable: "client",
                principalColumn: "client_id");

            migrationBuilder.AddForeignKey(
                name: "coach_client_coach_id",
                table: "coach_client",
                column: "coach_id",
                principalTable: "coach",
                principalColumn: "coach_id");
        }
    }
}
