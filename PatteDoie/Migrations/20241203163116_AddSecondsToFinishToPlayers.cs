using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class AddSecondsToFinishToPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("395bc859-4bae-45aa-9624-ac9150d7e818"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("7f45ba49-4cf4-44fb-b83a-f608a7c24179"));

            migrationBuilder.AddColumn<int>(
                name: "SecondsToFinish",
                table: "SpeedTypingPlayer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("be1cb1d7-1dfd-48df-8124-6fef68293ed5"), 8, 2, "Scattergories" },
                    { new Guid("f2b32db7-aca3-4572-99dd-fd4682c81af6"), 5, 1, "SpeedTyping" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("be1cb1d7-1dfd-48df-8124-6fef68293ed5"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("f2b32db7-aca3-4572-99dd-fd4682c81af6"));

            migrationBuilder.DropColumn(
                name: "SecondsToFinish",
                table: "SpeedTypingPlayer");

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("395bc859-4bae-45aa-9624-ac9150d7e818"), 8, 2, "Scattergories" },
                    { new Guid("7f45ba49-4cf4-44fb-b83a-f608a7c24179"), 5, 1, "SpeedTyping" }
                });
        }
    }
}
