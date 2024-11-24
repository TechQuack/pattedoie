using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class PlatformUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Min_players = table.Column<int>(type: "int", nullable: false),
                    Max_players = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScattergoriesGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxRound = table.Column<int>(type: "int", nullable: false),
                    CurrentRound = table.Column<int>(type: "int", nullable: false),
                    CurrentLetter = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattergoriesGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformHighScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Player = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false),
                    PlatformGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformHighScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformHighScore_PlatformGame_PlatformGameId",
                        column: x => x.PlatformGameId,
                        principalTable: "PlatformGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScattergoriesCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScattergoriesGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattergoriesCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScattergoriesCategory_ScattergoriesGame_ScattergoriesGameId",
                        column: x => x.ScattergoriesGameId,
                        principalTable: "ScattergoriesGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScattergoriesPlayer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ScattergoriesGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattergoriesPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScattergoriesPlayer_ScattergoriesGame_ScattergoriesGameId",
                        column: x => x.ScattergoriesGameId,
                        principalTable: "ScattergoriesGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformLobby",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    started = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformLobby", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatformLobbyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformUser_PlatformLobby_PlatformLobbyId",
                        column: x => x.PlatformLobbyId,
                        principalTable: "PlatformLobby",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformHighScore_PlatformGameId",
                table: "PlatformHighScore",
                column: "PlatformGameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformLobby_creatorId",
                table: "PlatformLobby",
                column: "creatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUser_PlatformLobbyId",
                table: "PlatformUser",
                column: "PlatformLobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesCategory_ScattergoriesGameId",
                table: "ScattergoriesCategory",
                column: "ScattergoriesGameId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesPlayer_ScattergoriesGameId",
                table: "ScattergoriesPlayer",
                column: "ScattergoriesGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformLobby_PlatformUser_creatorId",
                table: "PlatformLobby",
                column: "creatorId",
                principalTable: "PlatformUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformLobby_PlatformUser_creatorId",
                table: "PlatformLobby");

            migrationBuilder.DropTable(
                name: "PlatformHighScore");

            migrationBuilder.DropTable(
                name: "ScattergoriesCategory");

            migrationBuilder.DropTable(
                name: "ScattergoriesPlayer");

            migrationBuilder.DropTable(
                name: "PlatformGame");

            migrationBuilder.DropTable(
                name: "ScattergoriesGame");

            migrationBuilder.DropTable(
                name: "PlatformUser");

            migrationBuilder.DropTable(
                name: "PlatformLobby");
        }
    }
}
