using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionnaireApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Answer");
        }
    }
}
