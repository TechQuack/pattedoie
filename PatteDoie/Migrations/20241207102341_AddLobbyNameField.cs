using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class AddLobbyNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LobbyName",
                table: "PlatformLobby",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LobbyName",
                table: "PlatformLobby");
        }
    }
}
