#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

using Microsoft.EntityFrameworkCore.Migrations;

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class ScattergoriesCheckCurrentWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "ScattergoriesAnswer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "ScattergoriesAnswer");
        }
    }
}
