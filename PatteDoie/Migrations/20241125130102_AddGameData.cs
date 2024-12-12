#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

using Microsoft.EntityFrameworkCore.Migrations;

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class AddGameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("d26dfa84-fcb7-4ed6-a8be-6e520cc3c826"), 5, 1, "SpeedTyping" },
                    { new Guid("ff2415ff-a9a5-4679-bcbe-0a86ec741e72"), 8, 2, "Scattergories" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("d26dfa84-fcb7-4ed6-a8be-6e520cc3c826"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("ff2415ff-a9a5-4679-bcbe-0a86ec741e72"));
        }
    }
}
