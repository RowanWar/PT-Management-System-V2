using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToClientIdInWeeklyReportModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "weekly_report_user_id_fkey",
                table: "weekly_report");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "weekly_report");

            //migrationBuilder.RenameColumn(
            //    name: "user_id",
            //    table: "weekly_report",
            //    newName: "UserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_weekly_report_user_id",
            //    table: "weekly_report",
            //    newName: "IX_weekly_report_UserId");

            migrationBuilder.AddColumn<int>(
                name: "client_id",
                table: "weekly_report",
                type: "integer",
                nullable: true);

            // Use RAW SQL to fix this throwing an error relating to client_id containig null values upon running migration
            migrationBuilder.Sql("UPDATE weekly_report SET client_id = 1 WHERE client_id IS NULL;");

            // Makle client_id nullable after data is filled in
            migrationBuilder.AlterColumn<int>(
                name: "client_id",
                table: "weekly_report",
                type: "integer",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_report_client_id",
                table: "weekly_report",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_weekly_report_client_id",
                table: "weekly_report",
                column: "client_id",
                principalTable: "client",
                principalColumn: "client_id");

            //migrationBuilder.AddForeignKey(
            //    name: "weekly_report_user_id_fkey",
            //    table: "weekly_report",
            //    column: "client_id",
            //    principalTable: "client",
            //    principalColumn: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weekly_report_client_id",
                table: "weekly_report");

            migrationBuilder.DropIndex(
                name: "IX_weekly_report_client_id",
                table: "weekly_report");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "weekly_report");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "weekly_report",
                type: "integer",
                nullable: false,
                defaultValue: 0); // Adjust if nullable or needs a different default value

            migrationBuilder.AddForeignKey(
                name: "weekly_report_user_id_fkey",
                table: "weekly_report",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade); // Adjust as necessary based on original delete behavior

            //migrationBuilder.CreateIndex(
            //    name: "IX_weekly_report_user_id",
            //    table: "weekly_report",
            //    column: "user_id");
        }
    }
}
