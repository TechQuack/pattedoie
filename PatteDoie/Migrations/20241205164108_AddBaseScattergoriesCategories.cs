using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatteDoie.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseScattergoriesCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScattergoriesCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("009b2b44-43f8-4cb2-8a1e-d97e90ac3173"), "country" },
                    { new Guid("2ebe66c6-96f1-4ca0-aaf7-bee53c31f0d6"), "movie/tv show" },
                    { new Guid("412bca46-0c38-49e3-81a5-e166abe022d1"), "firstname" },
                    { new Guid("4db12c6f-a881-4222-ac1d-bb703a2c8975"), "animal" },
                    { new Guid("582acaaa-eed5-4ff3-8ca0-2f049ff9fef6"), "brand" },
                    { new Guid("5fc35a0e-6259-46b9-a436-473e1f04b5e4"), "fruit or vegetable" },
                    { new Guid("90dddb10-656f-4327-9b6f-2349edd07a35"), "clothes" },
                    { new Guid("a008c25d-e90c-4de4-b9cf-c960a31eeebd"), "famous person" },
                    { new Guid("aa86b4ed-20bf-480c-9027-dbb272fede5b"), "sport" },
                    { new Guid("ca79df4b-9a32-4787-9d2f-022ee6d5253f"), "occupation" },
                    { new Guid("d21597fe-9c39-4a0b-9104-97408b59ffd8"), "game" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("009b2b44-43f8-4cb2-8a1e-d97e90ac3173"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("2ebe66c6-96f1-4ca0-aaf7-bee53c31f0d6"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("412bca46-0c38-49e3-81a5-e166abe022d1"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("4db12c6f-a881-4222-ac1d-bb703a2c8975"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("582acaaa-eed5-4ff3-8ca0-2f049ff9fef6"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("5fc35a0e-6259-46b9-a436-473e1f04b5e4"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("90dddb10-656f-4327-9b6f-2349edd07a35"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("a008c25d-e90c-4de4-b9cf-c960a31eeebd"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("aa86b4ed-20bf-480c-9027-dbb272fede5b"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("ca79df4b-9a32-4787-9d2f-022ee6d5253f"));

            migrationBuilder.DeleteData(
                table: "ScattergoriesCategory",
                keyColumn: "Id",
                keyValue: new Guid("d21597fe-9c39-4a0b-9104-97408b59ffd8"));
        }
    }
}
