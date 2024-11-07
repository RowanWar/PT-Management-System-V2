using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT_Management_System_V2.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkoutProgramExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramExercise_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramExercise_exercise_ExerciseId",
                table: "WorkoutProgramExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutProgramExercise",
                table: "WorkoutProgramExercise");

            migrationBuilder.RenameTable(
                name: "WorkoutProgramExercise",
                newName: "WorkoutProgramExercises");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutProgramExercise_ExerciseId",
                table: "WorkoutProgramExercises",
                newName: "IX_WorkoutProgramExercises_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutProgramExercises",
                table: "WorkoutProgramExercises",
                columns: new[] { "WorkoutProgramId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramExercises_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramExercises",
                column: "WorkoutProgramId",
                principalTable: "WorkoutPrograms",
                principalColumn: "WorkoutProgramId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramExercises_exercise_ExerciseId",
                table: "WorkoutProgramExercises",
                column: "ExerciseId",
                principalTable: "exercise",
                principalColumn: "exercise_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramExercises_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutProgramExercises_exercise_ExerciseId",
                table: "WorkoutProgramExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutProgramExercises",
                table: "WorkoutProgramExercises");

            migrationBuilder.RenameTable(
                name: "WorkoutProgramExercises",
                newName: "WorkoutProgramExercise");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutProgramExercises_ExerciseId",
                table: "WorkoutProgramExercise",
                newName: "IX_WorkoutProgramExercise_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutProgramExercise",
                table: "WorkoutProgramExercise",
                columns: new[] { "WorkoutProgramId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramExercise_WorkoutPrograms_WorkoutProgramId",
                table: "WorkoutProgramExercise",
                column: "WorkoutProgramId",
                principalTable: "WorkoutPrograms",
                principalColumn: "WorkoutProgramId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutProgramExercise_exercise_ExerciseId",
                table: "WorkoutProgramExercise",
                column: "ExerciseId",
                principalTable: "exercise",
                principalColumn: "exercise_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
