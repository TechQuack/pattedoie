#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

using Microsoft.EntityFrameworkCore.Migrations;

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class SpeedTypingTimeProgressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "SpeedTypingTimeProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeProgress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpeedTypingGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedTypingTimeProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeedTypingTimeProgress_SpeedTypingGame_SpeedTypingGameId",
                        column: x => x.SpeedTypingGameId,
                        principalTable: "SpeedTypingGame",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpeedTypingTimeProgress_SpeedTypingPlayer_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "SpeedTypingPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingTimeProgress_PlayerId",
                table: "SpeedTypingTimeProgress",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeedTypingTimeProgress_SpeedTypingGameId",
                table: "SpeedTypingTimeProgress",
                column: "SpeedTypingGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeedTypingTimeProgress");

        }
    }
}
