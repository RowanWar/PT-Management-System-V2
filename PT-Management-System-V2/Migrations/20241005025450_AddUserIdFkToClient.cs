using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdFkToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_client_UserId",
                table: "client",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_AspNetUsers_UserId",
                table: "client",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_AspNetUsers_UserId",
                table: "client");

            migrationBuilder.DropIndex(
                name: "IX_client_UserId",
                table: "client");
        }
    }
}
