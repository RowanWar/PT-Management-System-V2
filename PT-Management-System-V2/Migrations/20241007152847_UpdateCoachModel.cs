using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoachModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // I've dropped this manually via Sql as it was causing issues with currently existing data and converting between previous Int type to the new <text> type in the db
            //migrationBuilder.DropForeignKey(
            //    name: "coach_coach_client_id_fkey",  // Old constraint name
            //    table: "coach");



            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "coach",
                type: "text",  // Change to 'text' type in PostgreSQL
                nullable: false,  // Or adjust nullability as needed
                oldClrType: typeof(int),  // Old type was integer
                oldType: "integer");  // Update old type to match



            migrationBuilder.AddForeignKey(
                name: "FK_Coach_AspNetUsers_Id",  // New constraint name
                table: "coach",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);



            //migrationBuilder.DropForeignKey(
            //    name: "FK_coach_AspNetUsers_user_id",
            //    table: "coach");

            //migrationBuilder.AddForeignKey(
            //    name: "Coach_AspNetUsers_Id_Fkey",
            //    table: "coach",
            //    column: "user_id",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Coach_AspNetUsers_Id_Fkey",
                table: "coach");

            migrationBuilder.AddForeignKey(
                name: "FK_coach_AspNetUsers_user_id",
                table: "coach",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
