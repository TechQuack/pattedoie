using System;
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
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("d26dfa84-fcb7-4ed6-a8be-6e520cc3c826"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("ff2415ff-a9a5-4679-bcbe-0a86ec741e72"));

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("2a13453e-2832-493c-9b69-82dc0f41890b"), 5, 1, "SpeedTyping" },
                    { new Guid("7de9aafd-9e6f-452c-af02-39bc938137f6"), 8, 2, "Scattergories" }
                });

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

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("2a13453e-2832-493c-9b69-82dc0f41890b"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("7de9aafd-9e6f-452c-af02-39bc938137f6"));

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("d26dfa84-fcb7-4ed6-a8be-6e520cc3c826"), 5, 1, "" },
                    { new Guid("ff2415ff-a9a5-4679-bcbe-0a86ec741e72"), 8, 2, "" }
                });
        }
    }
}
