using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDatabaseFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformHighScore_PlatformGame_PlatformGameId",
                table: "PlatformHighScore");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUser_PlatformLobby_PlatformLobbyId",
                table: "PlatformUser");

            migrationBuilder.RenameColumn(
                name: "PlatformLobbyId",
                table: "PlatformUser",
                newName: "LobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformUser_PlatformLobbyId",
                table: "PlatformUser",
                newName: "IX_PlatformUser_LobbyId");

            migrationBuilder.RenameColumn(
                name: "Player",
                table: "PlatformHighScore",
                newName: "PlayerName");

            migrationBuilder.RenameColumn(
                name: "PlatformGameId",
                table: "PlatformHighScore",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PlatformHighScore",
                newName: "Score");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformHighScore_PlatformGameId",
                table: "PlatformHighScore",
                newName: "IX_PlatformHighScore_GameId");

            migrationBuilder.RenameColumn(
                name: "Min_players",
                table: "PlatformGame",
                newName: "MinPlayers");

            migrationBuilder.RenameColumn(
                name: "Max_players",
                table: "PlatformGame",
                newName: "MaxPlayers");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformHighScore_PlatformGame_GameId",
                table: "PlatformHighScore",
                column: "GameId",
                principalTable: "PlatformGame",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUser_PlatformLobby_LobbyId",
                table: "PlatformUser",
                column: "LobbyId",
                principalTable: "PlatformLobby",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformHighScore_PlatformGame_GameId",
                table: "PlatformHighScore");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformUser_PlatformLobby_LobbyId",
                table: "PlatformUser");

            migrationBuilder.RenameColumn(
                name: "LobbyId",
                table: "PlatformUser",
                newName: "PlatformLobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformUser_LobbyId",
                table: "PlatformUser",
                newName: "IX_PlatformUser_PlatformLobbyId");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "PlatformHighScore",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PlayerName",
                table: "PlatformHighScore",
                newName: "Player");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "PlatformHighScore",
                newName: "PlatformGameId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformHighScore_GameId",
                table: "PlatformHighScore",
                newName: "IX_PlatformHighScore_PlatformGameId");

            migrationBuilder.RenameColumn(
                name: "MinPlayers",
                table: "PlatformGame",
                newName: "Min_players");

            migrationBuilder.RenameColumn(
                name: "MaxPlayers",
                table: "PlatformGame",
                newName: "Max_players");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformHighScore_PlatformGame_PlatformGameId",
                table: "PlatformHighScore",
                column: "PlatformGameId",
                principalTable: "PlatformGame",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformUser_PlatformLobby_PlatformLobbyId",
                table: "PlatformUser",
                column: "PlatformLobbyId",
                principalTable: "PlatformLobby",
                principalColumn: "Id");
        }
    }
}
