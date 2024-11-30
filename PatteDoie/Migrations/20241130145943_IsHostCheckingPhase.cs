using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class IsHostCheckingPhase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScattegoriesAnswer_ScattergoriesCategory_CategoryId",
                table: "ScattegoriesAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_ScattegoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                table: "ScattegoriesAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScattegoriesAnswer",
                table: "ScattegoriesAnswer");

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("4992bb85-e995-4cb1-88a5-ccf0c6613f02"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("73f91b38-f8d3-45a2-83dd-7320cff13bbb"));

            migrationBuilder.RenameTable(
                name: "ScattegoriesAnswer",
                newName: "ScattergoriesAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_ScattegoriesAnswer_ScattergoriesPlayerId",
                table: "ScattergoriesAnswer",
                newName: "IX_ScattergoriesAnswer_ScattergoriesPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ScattegoriesAnswer_CategoryId",
                table: "ScattergoriesAnswer",
                newName: "IX_ScattergoriesAnswer_CategoryId");

            migrationBuilder.AddColumn<bool>(
                name: "IsHostCheckingPhase",
                table: "ScattergoriesGame",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScattergoriesAnswer",
                table: "ScattergoriesAnswer",
                column: "Id");

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("2bb6ac65-b80c-4cee-b53d-d1f8496c6c9e"), 5, 1, "SpeedTyping" },
                    { new Guid("f6f00e0a-3448-4507-88e6-68beb19bb18e"), 8, 2, "Scattergories" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ScattergoriesAnswer_ScattergoriesCategory_CategoryId",
                table: "ScattergoriesAnswer",
                column: "CategoryId",
                principalTable: "ScattergoriesCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScattergoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                table: "ScattergoriesAnswer",
                column: "ScattergoriesPlayerId",
                principalTable: "ScattergoriesPlayer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScattergoriesAnswer_ScattergoriesCategory_CategoryId",
                table: "ScattergoriesAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_ScattergoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                table: "ScattergoriesAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScattergoriesAnswer",
                table: "ScattergoriesAnswer");

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("2bb6ac65-b80c-4cee-b53d-d1f8496c6c9e"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("f6f00e0a-3448-4507-88e6-68beb19bb18e"));

            migrationBuilder.DropColumn(
                name: "IsHostCheckingPhase",
                table: "ScattergoriesGame");

            migrationBuilder.RenameTable(
                name: "ScattergoriesAnswer",
                newName: "ScattegoriesAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_ScattergoriesAnswer_ScattergoriesPlayerId",
                table: "ScattegoriesAnswer",
                newName: "IX_ScattegoriesAnswer_ScattergoriesPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_ScattergoriesAnswer_CategoryId",
                table: "ScattegoriesAnswer",
                newName: "IX_ScattegoriesAnswer_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScattegoriesAnswer",
                table: "ScattegoriesAnswer",
                column: "Id");

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("4992bb85-e995-4cb1-88a5-ccf0c6613f02"), 5, 1, "SpeedTyping" },
                    { new Guid("73f91b38-f8d3-45a2-83dd-7320cff13bbb"), 8, 2, "Scattergories" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ScattegoriesAnswer_ScattergoriesCategory_CategoryId",
                table: "ScattegoriesAnswer",
                column: "CategoryId",
                principalTable: "ScattergoriesCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScattegoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                table: "ScattegoriesAnswer",
                column: "ScattergoriesPlayerId",
                principalTable: "ScattergoriesPlayer",
                principalColumn: "Id");
        }
    }
}
