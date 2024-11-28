using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyLobbyGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlatformLobby_GameId",
                table: "PlatformLobby",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformLobby_PlatformGame_GameId",
                table: "PlatformLobby",
                column: "GameId",
                principalTable: "PlatformGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformLobby_PlatformGame_GameId",
                table: "PlatformLobby");

            migrationBuilder.DropIndex(
                name: "IX_PlatformLobby_GameId",
                table: "PlatformLobby");
        }
    }
}
