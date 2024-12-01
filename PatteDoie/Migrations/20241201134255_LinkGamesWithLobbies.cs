using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class LinkGamesWithLobbies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScattergoriesCategory_ScattergoriesGame_ScattergoriesGameId",
                table: "ScattergoriesCategory");

            migrationBuilder.DropIndex(
                name: "IX_ScattergoriesCategory_ScattergoriesGameId",
                table: "ScattergoriesCategory");

            migrationBuilder.DropColumn(
                name: "ScattergoriesGameId",
                table: "ScattergoriesCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "LobbyId",
                table: "SpeedTypingGame",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LobbyId",
                table: "ScattergoriesGame",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ScattergoriesCategoryScattergoriesGame",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GamesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattergoriesCategoryScattergoriesGame", x => new { x.CategoriesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_ScattergoriesCategoryScattergoriesGame_ScattergoriesCategory_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "ScattergoriesCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScattergoriesCategoryScattergoriesGame_ScattergoriesGame_GamesId",
                        column: x => x.GamesId,
                        principalTable: "ScattergoriesGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingGame_LobbyId",
                table: "SpeedTypingGame",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesGame_LobbyId",
                table: "ScattergoriesGame",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesCategoryScattergoriesGame_GamesId",
                table: "ScattergoriesCategoryScattergoriesGame",
                column: "GamesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScattergoriesGame_PlatformLobby_LobbyId",
                table: "ScattergoriesGame",
                column: "LobbyId",
                principalTable: "PlatformLobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeedTypingGame_PlatformLobby_LobbyId",
                table: "SpeedTypingGame",
                column: "LobbyId",
                principalTable: "PlatformLobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScattergoriesGame_PlatformLobby_LobbyId",
                table: "ScattergoriesGame");

            migrationBuilder.DropForeignKey(
                name: "FK_SpeedTypingGame_PlatformLobby_LobbyId",
                table: "SpeedTypingGame");

            migrationBuilder.DropTable(
                name: "ScattergoriesCategoryScattergoriesGame");

            migrationBuilder.DropIndex(
                name: "IX_SpeedTypingGame_LobbyId",
                table: "SpeedTypingGame");

            migrationBuilder.DropIndex(
                name: "IX_ScattergoriesGame_LobbyId",
                table: "ScattergoriesGame");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "SpeedTypingGame");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "ScattergoriesGame");

            migrationBuilder.AddColumn<Guid>(
                name: "ScattergoriesGameId",
                table: "ScattergoriesCategory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesCategory_ScattergoriesGameId",
                table: "ScattergoriesCategory",
                column: "ScattergoriesGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScattergoriesCategory_ScattergoriesGame_ScattergoriesGameId",
                table: "ScattergoriesCategory",
                column: "ScattergoriesGameId",
                principalTable: "ScattergoriesGame",
                principalColumn: "Id");
        }
    }
}
