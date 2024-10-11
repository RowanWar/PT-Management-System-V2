using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_AspNetUsers_ApplicationUserId",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_client_AspNetUsers_UserId",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_Coach_AspNetUsers_Id",
                table: "coach");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_coach_client_CoachClientClientId",
            //    table: "coach");

            //migrationBuilder.DropIndex(
            //    name: "IX_coach_CoachClientClientId",
            //    table: "coach");

            //migrationBuilder.DropColumn(
            //    name: "CoachClientClientId",
            //    table: "coach");

            migrationBuilder.RenameColumn(
                name: "coach_qualifications",
                table: "coach",
                newName: "CoachQualifications");

            migrationBuilder.RenameColumn(
                name: "coach_profile_description",
                table: "coach",
                newName: "CoachProfileDescription");

            migrationBuilder.RenameColumn(
                name: "referred",
                table: "client",
                newName: "Referred");

            migrationBuilder.RenameColumn(
                name: "referral",
                table: "client",
                newName: "Referral");

            migrationBuilder.RenameColumn(
                name: "contact_by_phone",
                table: "client",
                newName: "ContactByPhone");

            migrationBuilder.RenameColumn(
                name: "contact_by_email",
                table: "client",
                newName: "ContactByEmail");

            migrationBuilder.AlterColumn<string>(
                name: "CoachQualifications",
                table: "coach",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoachProfileDescription",
                table: "coach",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Referral",
                table: "client",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "client",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_client_AspNetUsers_ApplicationUserId",
                table: "client",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coach_AspNetUsers_Id",
                table: "coach",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_AspNetUsers_ApplicationUserId",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_Coach_AspNetUsers_Id",
                table: "coach");

            migrationBuilder.RenameColumn(
                name: "CoachQualifications",
                table: "coach",
                newName: "coach_qualifications");

            migrationBuilder.RenameColumn(
                name: "CoachProfileDescription",
                table: "coach",
                newName: "coach_profile_description");

            migrationBuilder.RenameColumn(
                name: "Referred",
                table: "client",
                newName: "referred");

            migrationBuilder.RenameColumn(
                name: "Referral",
                table: "client",
                newName: "referral");

            migrationBuilder.RenameColumn(
                name: "ContactByPhone",
                table: "client",
                newName: "contact_by_phone");

            migrationBuilder.RenameColumn(
                name: "ContactByEmail",
                table: "client",
                newName: "contact_by_email");

            migrationBuilder.AlterColumn<string>(
                name: "coach_qualifications",
                table: "coach",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "coach_profile_description",
                table: "coach",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachClientClientId",
                table: "coach",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "referral",
                table: "client",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "client",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_coach_CoachClientClientId",
                table: "coach",
                column: "CoachClientClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_AspNetUsers_ApplicationUserId",
                table: "client",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_client_AspNetUsers_UserId",
                table: "client",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Coach_AspNetUsers_Id_Fkey",
                table: "coach",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_coach_client_CoachClientClientId",
                table: "coach",
                column: "CoachClientClientId",
                principalTable: "client",
                principalColumn: "client_id");
        }
    }
}
