using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnX_Data.Migrations
{
    /// <inheritdoc />
    public partial class updateEx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerKeyFilePath",
                table: "EssaySubmissions");

            migrationBuilder.AddColumn<string>(
                name: "AnswerFile",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Describe",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instruct",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerFile",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Describe",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Instruct",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "AnswerKeyFilePath",
                table: "EssaySubmissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
