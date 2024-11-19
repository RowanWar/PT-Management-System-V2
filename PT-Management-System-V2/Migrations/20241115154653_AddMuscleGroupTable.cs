using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddMuscleGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "muscle_group",
                table: "exercise",
                newName: "MuscleGroup");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroup",
                table: "exercise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "exercise",
                type: "integer",
                nullable: true);

            //migrationBuilder.Sql("UPDATE exercise SET \"MuscleGroupId\" = 1;");
            //migrationBuilder.Sql("UPDATE exercise SET MuscleGroupId = 1;");

            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                columns: table => new
                {
                    MuscleGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MuscleGroupName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MuscleGroup_pkey", x => x.MuscleGroupId);
                });

            migrationBuilder.InsertData(
                table: "MuscleGroup",
                columns: new[] { "MuscleGroupId", "MuscleGroupName" },
                values: new object[,]
                {
                                { 1, "Back" },
                                { 2, "Chest" },
                                { 3, "Legs" }
                });


            //migrationBuilder.Sql("UPDATE exercise SET \"MuscleGroupId\" = 1;");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_MuscleGroupId",
                table: "exercise",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_MuscleGroup",
                table: "exercise",
                column: "MuscleGroupId",
                principalTable: "MuscleGroup",
                principalColumn: "MuscleGroupId",
                onDelete: ReferentialAction.Restrict);

            

            //migrationBuilder.InsertData(
            //    table: "exercise",
            //    columns: new[] { "exercise_name", "description", "MuscleGroupId", "is_default" },
            //    values: new object[,]
            //    {
            //{ "Pull-Up", "A back-focused bodyweight exercise", 1, true },
            //{ "Bench Press", "A chest-focused weightlifting exercise", 2, true},
            //{ "Squat", "A leg-focused compound movement", 3, true }
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_MuscleGroup",
                table: "exercise");

            migrationBuilder.DropTable(
                name: "MuscleGroup");

            migrationBuilder.DropIndex(
                name: "IX_exercise_MuscleGroupId",
                table: "exercise");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "exercise");

            migrationBuilder.RenameColumn(
                name: "MuscleGroup",
                table: "exercise",
                newName: "muscle_group");

            migrationBuilder.AlterColumn<string>(
                name: "muscle_group",
                table: "exercise",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
