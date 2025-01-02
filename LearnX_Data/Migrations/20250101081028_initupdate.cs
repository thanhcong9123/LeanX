using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnX_Data.Migrations
{
    /// <inheritdoc />
    public partial class initupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Exercise_ExerciseId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_CourseId",
                table: "Exercises",
                newName: "IX_Exercises_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Courses_CourseId",
                table: "Exercises",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Exercises_ExerciseId",
                table: "Question",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Courses_CourseId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Exercises_ExerciseId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_CourseId",
                table: "Exercise",
                newName: "IX_Exercise_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Exercise_ExerciseId",
                table: "Question",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
