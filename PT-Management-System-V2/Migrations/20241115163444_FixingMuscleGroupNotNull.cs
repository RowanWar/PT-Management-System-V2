using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class FixingMuscleGroupNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MuscleGroupId",
                table: "exercise",
                type: "integer",
                nullable: false); // Enforce NOT NULL constraint
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
