using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceMuscleGroupWithFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetWorkoutProgramSchedules_WorkoutPrograms_WorkoutProgramId",
                table: "GetWorkoutProgramSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GetWorkoutProgramSchedules",
                table: "GetWorkoutProgramSchedules");

            migrationBuilder.DropColumn(
                name: "MuscleGroup",
                table: "GetWorkoutProgramSchedules");

            migrationBuilder.RenameTable(
                name: "GetWorkoutProgramSchedules",
                newName: "WorkoutProgramSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_GetWorkoutProgramSchedules_WorkoutProgramId",
                table: "WorkoutProgramSchedules",
                newName: "IX_WorkoutProgramSchedules_WorkoutProgramId");

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "WorkoutProgramSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "WorkoutSchedule_pkey",
                table: "WorkoutProgramSchedules",
                column: "WorkoutScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutProgramSchedules_MuscleGroupId",
                table: "WorkoutProgramSchedules",
                column: "MuscleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramSchedules_MuscleGroup_MuscleGroupId",
                table: "WorkoutProgramSchedules",
                column: "MuscleGroupId",
                principalTable: "MuscleGroup",
                principalColumn: "MuscleGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramSchedules_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramSchedules",
                column: "WorkoutProgramId",
                principalTable: "WorkoutPrograms",
                principalColumn: "WorkoutProgramId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramSchedules_MuscleGroup_MuscleGroupId",
                table: "WorkoutProgramSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramSchedules_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "WorkoutSchedule_pkey",
                table: "WorkoutProgramSchedules");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutProgramSchedules_MuscleGroupId",
                table: "WorkoutProgramSchedules");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "WorkoutProgramSchedules");

            migrationBuilder.RenameTable(
                name: "WorkoutProgramSchedules",
                newName: "GetWorkoutProgramSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutProgramSchedules_WorkoutProgramId",
                table: "GetWorkoutProgramSchedules",
                newName: "IX_GetWorkoutProgramSchedules_WorkoutProgramId");

            migrationBuilder.AddColumn<string>(
                name: "MuscleGroup",
                table: "GetWorkoutProgramSchedules",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetWorkoutProgramSchedules",
                table: "GetWorkoutProgramSchedules",
                column: "WorkoutScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetWorkoutProgramSchedules_WorkoutPrograms_WorkoutProgramId",
                table: "GetWorkoutProgramSchedules",
                column: "WorkoutProgramId",
                principalTable: "WorkoutPrograms",
                principalColumn: "WorkoutProgramId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
