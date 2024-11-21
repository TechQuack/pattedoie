using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkPlayerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeedTypingScore");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ScattergoriesPlayer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SpeedTypingPlayer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeedTypingGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedTypingPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeedTypingPlayer_PlatformUser_UserId",
                        column: x => x.UserId,
                        principalTable: "PlatformUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeedTypingPlayer_SpeedTypingGame_SpeedTypingGameId",
                        column: x => x.SpeedTypingGameId,
                        principalTable: "SpeedTypingGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesPlayer_UserId",
                table: "ScattergoriesPlayer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingPlayer_SpeedTypingGameId",
                table: "SpeedTypingPlayer",
                column: "SpeedTypingGameId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingPlayer_UserId",
                table: "SpeedTypingPlayer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScattergoriesPlayer_PlatformUser_UserId",
                table: "ScattergoriesPlayer",
                column: "UserId",
                principalTable: "PlatformUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScattergoriesPlayer_PlatformUser_UserId",
                table: "ScattergoriesPlayer");

            migrationBuilder.DropTable(
                name: "SpeedTypingPlayer");

            migrationBuilder.DropIndex(
                name: "IX_ScattergoriesPlayer_UserId",
                table: "ScattergoriesPlayer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ScattergoriesPlayer");

            migrationBuilder.CreateTable(
                name: "SpeedTypingScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    SpeedTypingGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedTypingScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeedTypingScore_SpeedTypingGame_SpeedTypingGameId",
                        column: x => x.SpeedTypingGameId,
                        principalTable: "SpeedTypingGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingScore_SpeedTypingGameId",
                table: "SpeedTypingScore",
                column: "SpeedTypingGameId");
        }
    }
}
