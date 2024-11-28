using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class ScattergoriesCheckCurrentWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("acc87108-7333-4152-a762-ac7f5c7fa750"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("f750be83-efe7-41f5-bcd0-8f16f3a7b793"));

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "ScattegoriesAnswer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("4992bb85-e995-4cb1-88a5-ccf0c6613f02"), 5, 1, "SpeedTyping" },
                    { new Guid("73f91b38-f8d3-45a2-83dd-7320cff13bbb"), 8, 2, "Scattergories" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("4992bb85-e995-4cb1-88a5-ccf0c6613f02"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("73f91b38-f8d3-45a2-83dd-7320cff13bbb"));

            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "ScattegoriesAnswer");

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("acc87108-7333-4152-a762-ac7f5c7fa750"), 5, 1, "SpeedTyping" },
                    { new Guid("f750be83-efe7-41f5-bcd0-8f16f3a7b793"), 8, 2, "Scattergories" }
                });
        }
    }
}
