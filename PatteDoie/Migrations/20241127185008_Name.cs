using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("2a13453e-2832-493c-9b69-82dc0f41890b"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("7de9aafd-9e6f-452c-af02-39bc938137f6"));

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "ScattergoriesPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ScattegoriesAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScattergoriesPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattegoriesAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScattegoriesAnswer_ScattergoriesCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScattergoriesCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScattegoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                        column: x => x.ScattergoriesPlayerId,
                        principalTable: "ScattergoriesPlayer",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("7d635c33-abec-48c8-9bf2-d2d5940aa889"), 5, 1, "SpeedTyping" },
                    { new Guid("a138ec75-3bb1-4544-9db4-915e6c560c90"), 8, 2, "Scattergories" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScattegoriesAnswer_CategoryId",
                table: "ScattegoriesAnswer",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattegoriesAnswer_ScattergoriesPlayerId",
                table: "ScattegoriesAnswer",
                column: "ScattergoriesPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScattegoriesAnswer");

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("7d635c33-abec-48c8-9bf2-d2d5940aa889"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("a138ec75-3bb1-4544-9db4-915e6c560c90"));

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "ScattergoriesPlayer");

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("2a13453e-2832-493c-9b69-82dc0f41890b"), 5, 1, "SpeedTyping" },
                    { new Guid("7de9aafd-9e6f-452c-af02-39bc938137f6"), 8, 2, "Scattergories" }
                });
        }
    }
}
