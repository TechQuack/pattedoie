using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class SpeedTypingGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpeedTypingGameId",
                table: "SpeedTypingScore",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpeedTypingGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LaunchTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedTypingGame", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingScore_SpeedTypingGameId",
                table: "SpeedTypingScore",
                column: "SpeedTypingGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpeedTypingScore_SpeedTypingGame_SpeedTypingGameId",
                table: "SpeedTypingScore",
                column: "SpeedTypingGameId",
                principalTable: "SpeedTypingGame",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpeedTypingScore_SpeedTypingGame_SpeedTypingGameId",
                table: "SpeedTypingScore");

            migrationBuilder.DropTable(
                name: "SpeedTypingGame");

            migrationBuilder.DropIndex(
                name: "IX_SpeedTypingScore_SpeedTypingGameId",
                table: "SpeedTypingScore");

            migrationBuilder.DropColumn(
                name: "SpeedTypingGameId",
                table: "SpeedTypingScore");
        }
    }
}
