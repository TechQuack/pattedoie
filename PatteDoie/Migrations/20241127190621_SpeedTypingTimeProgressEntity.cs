using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class SpeedTypingTimeProgressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("7d635c33-abec-48c8-9bf2-d2d5940aa889"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("a138ec75-3bb1-4544-9db4-915e6c560c90"));

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("d110dad8-2625-42cc-9412-44393c5c9658"), 8, 2, "Scattergories" },
                    { new Guid("e4d96495-5e08-489b-9fc3-6541ab07140d"), 5, 1, "SpeedTyping" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("d110dad8-2625-42cc-9412-44393c5c9658"));

            migrationBuilder.DeleteData(
                table: "PlatformGame",
                keyColumn: "Id",
                keyValue: new Guid("e4d96495-5e08-489b-9fc3-6541ab07140d"));

            migrationBuilder.InsertData(
                table: "PlatformGame",
                columns: new[] { "Id", "MaxPlayers", "MinPlayers", "Name" },
                values: new object[,]
                {
                    { new Guid("7d635c33-abec-48c8-9bf2-d2d5940aa889"), 5, 1, "SpeedTyping" },
                    { new Guid("a138ec75-3bb1-4544-9db4-915e6c560c90"), 8, 2, "Scattergories" }
                });
        }
    }
}
