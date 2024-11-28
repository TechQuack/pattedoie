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

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "ScattergoriesPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ScattergoriesAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScattergoriesPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScattergoriesAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScattergoriesAnswer_ScattergoriesCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScattergoriesCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScattergoriesAnswer_ScattergoriesPlayer_ScattergoriesPlayerId",
                        column: x => x.ScattergoriesPlayerId,
                        principalTable: "ScattergoriesPlayer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesAnswer_CategoryId",
                table: "ScattergoriesAnswer",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScattergoriesAnswer_ScattergoriesPlayerId",
                table: "ScattergoriesAnswer",
                column: "ScattergoriesPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScattergoriesAnswer");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "ScattergoriesPlayer");

        }
    }
}
