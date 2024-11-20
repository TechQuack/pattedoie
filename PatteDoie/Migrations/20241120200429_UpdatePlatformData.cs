using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlatformData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformLobby_PlatformUser_creatorId",
                table: "PlatformLobby");

            migrationBuilder.RenameColumn(
                name: "started",
                table: "PlatformLobby",
                newName: "Started");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "PlatformLobby",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "gameId",
                table: "PlatformLobby",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "creatorId",
                table: "PlatformLobby",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformLobby_creatorId",
                table: "PlatformLobby",
                newName: "IX_PlatformLobby_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "PlatformLobby",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformLobby_PlatformUser_CreatorId",
                table: "PlatformLobby",
                column: "CreatorId",
                principalTable: "PlatformUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlatformLobby_PlatformUser_CreatorId",
                table: "PlatformLobby");

            migrationBuilder.RenameColumn(
                name: "Started",
                table: "PlatformLobby",
                newName: "started");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "PlatformLobby",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "PlatformLobby",
                newName: "gameId");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "PlatformLobby",
                newName: "creatorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlatformLobby_CreatorId",
                table: "PlatformLobby",
                newName: "IX_PlatformLobby_creatorId");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "PlatformLobby",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformLobby_PlatformUser_creatorId",
                table: "PlatformLobby",
                column: "creatorId",
                principalTable: "PlatformUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
