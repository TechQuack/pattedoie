using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SpeedTypingTimeProgress",
                newName: "PlayerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeProgress",
                table: "SpeedTypingTimeProgress",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingTimeProgress_PlayerId",
                table: "SpeedTypingTimeProgress",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpeedTypingTimeProgress_SpeedTypingPlayer_PlayerId",
                table: "SpeedTypingTimeProgress",
                column: "PlayerId",
                principalTable: "SpeedTypingPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpeedTypingTimeProgress_SpeedTypingPlayer_PlayerId",
                table: "SpeedTypingTimeProgress");

            migrationBuilder.DropIndex(
                name: "IX_SpeedTypingTimeProgress_PlayerId",
                table: "SpeedTypingTimeProgress");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "SpeedTypingTimeProgress",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "TimeProgress",
                table: "SpeedTypingTimeProgress",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
