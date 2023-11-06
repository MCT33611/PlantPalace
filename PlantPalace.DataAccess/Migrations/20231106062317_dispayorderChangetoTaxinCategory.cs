using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dispayorderChangetoTaxinCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "Categorries",
                newName: "Tax");

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 1,
                column: "Tax",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 2,
                column: "Tax",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 3,
                column: "Tax",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 4,
                column: "Tax",
                value: 18);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tax",
                table: "Categorries",
                newName: "DisplayOrder");

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DisplayOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categorries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DisplayOrder",
                value: 1);
        }
    }
}
